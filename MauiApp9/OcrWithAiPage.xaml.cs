using System.Text;
using System.Text.Json;

namespace MauiApp9;

public partial class OcrWithAiPage : ContentPage
{
    private byte[] _selectedImageBytes;
    private HttpClient _httpClient;

    public OcrWithAiPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _httpClient = new HttpClient();
    }

    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    await ProcessSelectedPhoto(photo);
                }
            }
            else
            {
                await DisplayAlert("Error", "Camera capture is not supported on this device", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to take photo: {ex.Message}", "OK");
        }
    }

    private async void OnPickPhotoClicked(object sender, EventArgs e)
    {
        try
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
                        await ProcessSelectedPhoto(photo);
                    });
                },
            }.Show(this);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to pick photo: {ex.Message}", "OK");
        }
    }

    private async Task ProcessSelectedPhoto(FileResult photo)
    {
        try
        {
            // Read the image as bytes
            using var stream = await photo.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            _selectedImageBytes = memoryStream.ToArray();

            // Display the image
            SelectedImage.Source = ImageSource.FromStream(
                () => new MemoryStream(_selectedImageBytes)
            );

            // Enable the process button
            ProcessButton.IsEnabled = true;

            // Clear previous OCR result
            OcrResultLabel.Text = "OCR result will appear here...";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to process selected photo: {ex.Message}", "OK");
        }
    }

    private async void OnProcessOcrClicked(object sender, EventArgs e)
    {
        if (_selectedImageBytes == null || _selectedImageBytes.Length == 0)
        {
            await DisplayAlert("Error", "Please select an image first", "OK");
            return;
        }

        try
        {
            // Show loading indicator
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            ProcessButton.IsEnabled = false;
            OcrResultLabel.Text = "Processing OCR...";

            // Call the OCR API
            var result = await CallOcrApi(_selectedImageBytes);

            // Display the result
            OcrResultLabel.Text = string.IsNullOrEmpty(result)
                ? "No text detected in the image."
                : result;
        }
        catch (Exception ex)
        {
            OcrResultLabel.Text = $"Error processing OCR: {ex.Message}";
            await DisplayAlert("Error", $"Failed to process OCR: {ex.Message}", "OK");
        }
        finally
        {
            // Hide loading indicator
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            ProcessButton.IsEnabled = true;
        }
    }

    private async Task<string> CallOcrApi(byte[] imageBytes)
    {
        try
        {
            // var apiUrl = "http://192.168.68.57:5678/webhook-test/ocr";
            var apiUrl = "http://192.168.68.57:5678/webhook/ocr";

            // Create multipart form data content
            using var content = new MultipartFormDataContent();
            using var imageContent = new ByteArrayContent(imageBytes);
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(
                "image/jpeg"
            );
            content.Add(imageContent, "data", "image.jpg");

            // Send POST request
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                // Try to parse the specific JSON format returned by your API
                try
                {
                    using var jsonDoc = JsonDocument.Parse(responseContent);

                    // Check if it's an array
                    if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        var array = jsonDoc.RootElement.EnumerateArray();
                        var firstElement = array.FirstOrDefault();

                        if (
                            firstElement.ValueKind != JsonValueKind.Undefined
                            && firstElement.TryGetProperty("output", out var outputElement)
                        )
                        {
                            return outputElement.GetString() ?? "No output found in response.";
                        }
                    }

                    // Fallback: look for common OCR response fields
                    if (jsonDoc.RootElement.TryGetProperty("text", out var textElement))
                    {
                        return textElement.GetString() ?? responseContent;
                    }
                    else if (jsonDoc.RootElement.TryGetProperty("result", out var resultElement))
                    {
                        return resultElement.GetString() ?? responseContent;
                    }
                    else if (
                        jsonDoc.RootElement.TryGetProperty(
                            "extractedText",
                            out var extractedTextElement
                        )
                    )
                    {
                        return extractedTextElement.GetString() ?? responseContent;
                    }
                    else
                    {
                        // Return the full JSON response if no specific text field is found
                        return responseContent;
                    }
                }
                catch (JsonException)
                {
                    // If it's not JSON, return as plain text
                    return responseContent;
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"API call failed with status {response.StatusCode}: {errorContent}"
                );
            }
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error calling OCR API: {ex.Message}", ex);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _httpClient?.Dispose();
    }
}
