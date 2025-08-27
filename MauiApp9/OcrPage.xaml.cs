using System.Text.Json;
// using Emgu.CV;
// using Emgu.CV.CvEnum;
// using Emgu.CV.Structure;
// using Emgu.CV.Util;
// using Emgu.CV.XPhoto;
using Plugin.Maui.OCR;
using Plugin.Maui.OCR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Color = Microsoft.Maui.Graphics.Color;
using Image = SixLabors.ImageSharp.Image;
#if ANDROID
using Android.Graphics;
#else
using UIKit;
using Foundation;
#endif

namespace MauiApp9;

public partial class OcrPage : ContentPage
{
    private readonly IOcrService _ocr;
    private byte[] _originalImageData;
    private byte[] _preprocessedImageData;
    private HttpClient _httpClient;

    public OcrPage(IOcrService feature)
    {
        InitializeComponent();

        _ocr = feature;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _ocr.InitAsync();
        _httpClient = new HttpClient();
    }

    private void CameraTabBtn_Clicked(object sender, EventArgs e)
    {
        CameraTabBtn.BackgroundColor = Colors.DodgerBlue;
        CameraTabBtn.TextColor = Colors.White;
        FileTabBtn.BackgroundColor = Colors.LightGray;
        FileTabBtn.TextColor = Colors.Black;

        CameraOptions.IsVisible = true;
        FileOptions.IsVisible = false;
    }

    private void FileTabBtn_Clicked(object sender, EventArgs e)
    {
        FileTabBtn.BackgroundColor = Colors.Green;
        FileTabBtn.TextColor = Colors.White;
        CameraTabBtn.BackgroundColor = Colors.LightGray;
        CameraTabBtn.TextColor = Colors.Black;

        FileOptions.IsVisible = true;
        CameraOptions.IsVisible = false;
    }

    /// <summary>
    /// Shows the loading indicator
    /// </summary>
    private void ShowLoading(string message = "Processing...")
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
        });
    }

    /// <summary>
    /// Hides the loading indicator
    /// </summary>
    private void HideLoading()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        });
    }

    private void ClearBtn_Clicked(object sender, EventArgs e)
    {
        ResultLbl.Text = "Waiting for results ...";
        CapturedImage.Source = null;
        NoImagePlaceholder.IsVisible = true;
        _originalImageData = null;
        _preprocessedImageData = null;
        AiResponseLbl.Text = "AI response will appear here...";
    }

    private async void CopyBtn_Clicked(object sender, EventArgs e)
    {
        if (ResultLbl.Text != "Waiting for results ...")
        {
            // Remove any HTML tags if present
            var plainText = ResultLbl.Text.Replace("<b>", "").Replace("</b>", "");
            await Clipboard.SetTextAsync(plainText);

            // Optional: Show feedback to user
            await DisplayAlert("Success", "Text copied to clipboard", "OK");
        }
    }

    private async void AnalyzeWithAiBtn_Clicked(object sender, EventArgs e)
    {
        if (ResultLbl.Text == "Waiting for results ...")
            return;

        try
        {
            ShowLoading("Analyzing with AI...");

            var aiResult = await CallOcrApi(ResultLbl.Text);

            AiResponseLbl.Text = FormatJsonForDisplay(aiResult);
            ;
        }
        catch (Exception ex)
        {
            AiResponseLbl.Text = $"Error: {ex.Message}";
        }
        finally
        {
            HideLoading();
        }
    }

    public string FormatJsonForDisplay(string rawResponse)
    {
        // Step 1: Extract the "output" string
        var jsonArray = JsonDocument.Parse(rawResponse).RootElement;
        var outputString = jsonArray[0].GetProperty("output").GetString();

        // Step 2: Parse the string as JSON again
        var parsedJson = JsonDocument.Parse(outputString);

        // Step 3: Reformat it with indentation
        var formatted = JsonSerializer.Serialize(
            parsedJson,
            new JsonSerializerOptions { WriteIndented = true }
        );

        return formatted;
    }

    private async Task<string> CallOcrApi(string text)
    {
        // var apiUrl = "http://192.168.68.57:5678/webhook-test/ocrdata";
        var apiUrl = "http://192.168.68.57:5678/webhook/ocrdata";

        using var client = new HttpClient();
        var json = JsonSerializer.Serialize(new { text });
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync(apiUrl, content);
        return await response.Content.ReadAsStringAsync();
    }

    private async void EnhanceImageBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (_originalImageData == null)
            {
                return;
            }

            ShowLoading("Enhancing image...");

            // Show loading indicator or disable button
            EnhanceImageBtn.IsEnabled = false;

            try
            {
                // Try different preprocessing parameters
                // _preprocessedImageData = ApplyAdvancedPreprocessing(_originalImageData);

                // Display the enhanced image
                await DisplayImage(_preprocessedImageData);

                // Re-run OCR on the enhanced image
                ShowLoading("Running OCR on enhanced image...");
                var options = new OcrOptions.Builder().SetTryHard(TryHardSwitch.IsToggled).Build();
                var result = await _ocr.RecognizeTextAsync(_preprocessedImageData, options);

                // Update the results
                ResultLbl.Text = result.AllText;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to enhance image: {ex.Message}", "OK");
            }
            finally
            {
                // Re-enable button
                EnhanceImageBtn.IsEnabled = true;
            }
        }
        finally
        {
            HideLoading();
        }
    }

    private async void OpenFromCameraBtn_Clicked(object sender, EventArgs e)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            var photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo == null)
            {
                return;
            }

            var result = await ProcessPhoto(photo);
            ResultLbl.Text = result.AllText;
            NoImagePlaceholder.IsVisible = false;
        }
        else
        {
            await DisplayAlert("Sorry", "Image capture is not supported on this device.", "OK");
        }
    }

    private async void OpenFromCameraUseEventBtn_Clicked(object sender, EventArgs e)
    {
        new ImageCropper.Maui.ImageCropper()
        {
            // PageTitle = LocalizationResourceManager["CropPageTitle"].ToString(),
            // AspectRatioX = 1,
            // AspectRatioY = 1,
            CropShape = ImageCropper.Maui.ImageCropper.CropShapeType.Rectangle,
            // SelectSourceTitle = LocalizationResourceManager["SelectOption"].ToString(),
            // TakePhotoTitle = LocalizationResourceManager["PickPhoto"].ToString(),
            // PhotoLibraryTitle = LocalizationResourceManager["TakePicture"].ToString(),
            // CancelButtonTitle = LocalizationResourceManager["Cancel"].ToString(),
            Success = async (imageFile) =>
            {
                await Dispatcher.DispatchAsync(async () =>
                {
                    var photo = new FileResult(imageFile);
                    _ocr.RecognitionCompleted += OnRecognitionCompleted;
                    await StartProcessingPhoto(photo);
                });
            },
        }.Show(this);
    }

    private async void OpenFromFileBtn_Clicked(object sender, EventArgs e)
    {
        var photo = await MediaPicker.Default.PickPhotoAsync();

        if (photo != null)
        {
            var result = await ProcessPhoto(photo);
            ResultLbl.Text = result.AllText;
            NoImagePlaceholder.IsVisible = false;
        }
    }

    private async void OpenFromFileUseEventBtn_Clicked(object sender, EventArgs e)
    {
        var photo = await MediaPicker.Default.PickPhotoAsync();

        if (photo == null)
        {
            return;
        }

        _ocr.RecognitionCompleted += OnRecognitionCompleted;
        await StartProcessingPhoto(photo);
    }

    private void OnRecognitionCompleted(object? sender, OcrCompletedEventArgs e)
    {
        // Remove the event handler to avoid multiple subscriptions
        _ocr.RecognitionCompleted -= OnRecognitionCompleted;

        // Update UI on the main thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ResultLbl.Text = e is { IsSuccessful: true, Result: not null }
                ? e.Result.AllText
                : $"Error: {(!string.IsNullOrEmpty(e.ErrorMessage) ? e.ErrorMessage : "Unknown")}";

            NoImagePlaceholder.IsVisible = false;
        });
    }

    /// <summary>
    ///     Takes a photo and processes it using the OCR service.
    /// </summary>
    /// <param name="photo">The photo to process.</param>
    /// <returns>The OCR result.</returns>
    private async Task<OcrResult> ProcessPhoto(FileResult photo)
    {
        try
        {
            ShowLoading("Reading image...");

            // Open a stream to the photo
            await using var sourceStream = await photo.OpenReadAsync();

            // Create a byte array to hold the image data
            _originalImageData = new byte[sourceStream.Length];

            using var cancellationTokenSource = new CancellationTokenSource(
                TimeSpan.FromSeconds(5)
            );

            // Read the stream into the byte array
            var dataLength = await sourceStream
                .ReadAsync(_originalImageData, cancellationTokenSource.Token)
                .ConfigureAwait(false);

            if (dataLength <= 0)
            {
                throw new InvalidOperationException("No image bytes");
            }

            ShowLoading("Preprocessing image...");
            _preprocessedImageData = await PreprocessImageForOcr(_originalImageData);

            await DisplayImage(_preprocessedImageData);

            var options = new OcrOptions.Builder().SetTryHard(TryHardSwitch.IsToggled).Build();

            // Process the image data using the OCR service
            ShowLoading("Extracting text...");
            return await _ocr.RecognizeTextAsync(
                _preprocessedImageData,
                options,
                cancellationTokenSource.Token
            );
        }
        finally
        {
            HideLoading();
        }
    }

    private async Task DisplayImage(byte[] imageData)
    {
        await Dispatcher.DispatchAsync(() =>
        {
            CapturedImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
            NoImagePlaceholder.IsVisible = false;
        });
    }

    private static async Task<byte[]> PreprocessImageForOcr(byte[] imageData)
    {
        await using var ms = new MemoryStream(imageData);
        try
        {
            var ret = await Image.IdentifyAsync(ms);
            using var image = await Image.LoadAsync<L8>(ms);

            // using var image = await Image.LoadAsync<Rgba32>(ms);
            const int MaxDimension = 1500;
            if (image.Width > MaxDimension || image.Height > MaxDimension)
            {
                var ratio = Math.Min(
                    (float)MaxDimension / image.Width,
                    (float)MaxDimension / image.Height
                );
                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);
                image.Mutate(x => x.Resize(newWidth, newHeight));
            }

            // Apply preprocessing steps
            image.Mutate(x =>
                x.Contrast(1.2f) // Slight contrast boost
                    .GaussianBlur(1.1f) // Slight blur to reduce dot matrix noise
                    .BinaryThreshold(0.45f) // Binarize at slightly lower threshold to preserve thin text
            );

            image.Mutate(x => x.Pad(10, 10));

            // Convert back to byte array
            using var resultMs = new MemoryStream();
            await image.SaveAsPngAsync(resultMs);
            return resultMs.ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }

    // private static byte[] ApplyAdvancedPreprocessing(byte[] imageData)
    // {
    //     // Load image as grayscale
    //     using var grayscaled = new Mat();
    //     CvInvoke.Imdecode(imageData, ImreadModes.Grayscale, grayscaled);
    //
    //     // Ensure it's upright first
    //     if (grayscaled.Width > grayscaled.Height)
    //         CvInvoke.Rotate(grayscaled, grayscaled, RotateFlags.Rotate90Clockwise);
    //
    //     // 1. Apply CLAHE (adaptive histogram equalization)
    //     using var clahed = new Mat();
    //     CvInvoke.CLAHE(
    //         grayscaled,
    //         clipLimit: 2.0,
    //         tileGridSize: new System.Drawing.Size(8, 8),
    //         clahed
    //     );
    //
    //     // 2. Gaussian Blur to smooth noise slightly
    //     CvInvoke.MedianBlur(clahed, clahed, 3);
    //
    //     //using var deskewed = Deskew(clahed);
    //
    //     // 3. Adaptive Thresholding (better for uneven lighting)
    //     using var thresholded = new Mat();
    //     CvInvoke.AdaptiveThreshold(
    //         clahed,
    //         thresholded,
    //         255,
    //         AdaptiveThresholdType.MeanC,
    //         ThresholdType.BinaryInv,
    //         15,
    //         10
    //     );
    //
    //     // 4. Morphological Open + Close to remove noise and fill gaps
    //     using var morphKernel = CvInvoke.GetStructuringElement(
    //         ElementShape.Rectangle,
    //         new System.Drawing.Size(2, 2),
    //         new System.Drawing.Point(-1, -1)
    //     );
    //     CvInvoke.MorphologyEx(
    //         thresholded,
    //         thresholded,
    //         MorphOp.Open,
    //         morphKernel,
    //         new System.Drawing.Point(-1, -1),
    //         1,
    //         BorderType.Reflect,
    //         new MCvScalar()
    //     );
    //     CvInvoke.MorphologyEx(
    //         thresholded,
    //         thresholded,
    //         MorphOp.Close,
    //         morphKernel,
    //         new System.Drawing.Point(-1, -1),
    //         1,
    //         BorderType.Reflect,
    //         new MCvScalar()
    //     );
    //
    //     // Return as byte array
    //     return thresholded.ToImage<Gray, byte>().ToJpegData();
    // }
    //
    // private static Mat Deskew(Mat src)
    // {
    //     var isPortrait = src.Height > src.Width;
    //     if (!isPortrait)
    //     {
    //         // Rotate 90 degrees clockwise to get back to portrait
    //         CvInvoke.Rotate(src, src, RotateFlags.Rotate90Clockwise);
    //     }
    //
    //     // Threshold to detect edges
    //     using var binary = new Mat();
    //     CvInvoke.Threshold(src, binary, 0, 255, ThresholdType.BinaryInv | ThresholdType.Otsu);
    //
    //     // Find contours
    //     using var contours = new VectorOfVectorOfPoint();
    //     CvInvoke.FindContours(
    //         binary,
    //         contours,
    //         null,
    //         RetrType.External,
    //         ChainApproxMethod.ChainApproxSimple
    //     );
    //
    //     // Combine all contour points into one big array
    //     var allPoints = contours.ToArrayOfArray().SelectMany(p => p).ToArray();
    //     if (allPoints.Length == 0)
    //         return src.Clone(); // No contours found
    //
    //     using var pts = new VectorOfPoint(allPoints);
    //     var box = CvInvoke.MinAreaRect(pts);
    //
    //     double angle = box.Angle;
    //     if (angle < -45)
    //         angle += 90;
    //
    //     // Rotate image to correct angle
    //     var center = new System.Drawing.PointF(src.Width / 2f, src.Height / 2f);
    //     using var rotationMatrix = new Mat();
    //     CvInvoke.GetRotationMatrix2D(center, angle, 1.0, rotationMatrix);
    //
    //     using var rotated = new Mat();
    //     CvInvoke.WarpAffine(
    //         src,
    //         rotated,
    //         rotationMatrix,
    //         src.Size,
    //         Inter.Linear,
    //         Warp.Default,
    //         BorderType.Constant,
    //         new MCvScalar(255)
    //     );
    //
    //     return rotated.Clone();
    // }

    /// <summary>
    ///     Takes a photo and starts processing it using the OCR service with events.
    /// </summary>
    /// <param name="photo">The photo to process.</param>
    private async Task StartProcessingPhoto(FileResult photo)
    {
        // Open a stream to the photo
        await using var sourceStream = await photo.OpenReadAsync();

        // Create a byte array to hold the image data
        _originalImageData = new byte[sourceStream.Length];

        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Read the stream into the byte array
        var dataLength = await sourceStream.ReadAsync(
            _originalImageData,
            cancellationTokenSource.Token
        );

        if (dataLength <= 0)
        {
            throw new InvalidOperationException("No image bytes");
        }

        _preprocessedImageData = await PreprocessImageForOcr(_originalImageData);

        await DisplayImage(_preprocessedImageData ?? _originalImageData);

        // Process the image data using the OCR service
        await _ocr.StartRecognizeTextAsync(
            _preprocessedImageData ?? _originalImageData,
            new OcrOptions.Builder().SetTryHard(TryHardSwitch.IsToggled).Build(),
            cancellationTokenSource.Token
        );
    }

#if IOS
    public static async Task<Stream> ReencodeImageAsync(FileResult photo)
    {
        if (photo == null)
            return null;

        // Load NSData from the photo file path
        using var nsData = NSData.FromUrl(NSUrl.FromFilename(photo.FullPath));
        using var uiImage = UIImage.LoadFromData(nsData);

        if (uiImage == null)
            return null;

        // Convert to PNG to clean metadata and color space
        using var pngData = uiImage.AsPNG();
        return pngData.AsStream();
    }
#endif
#if ANDROID
    public static async Task<Stream> ReencodeImageAsync(FileResult photo)
    {
        if (photo == null)
            return null;

        using var bitmap = BitmapFactory.DecodeFile(photo.FullPath);
        if (bitmap == null)
            return null;

        var ms = new MemoryStream();
        bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, ms); // PNG for lossless and clean alpha
        ms.Position = 0;
        return ms;
    }
#endif
}
