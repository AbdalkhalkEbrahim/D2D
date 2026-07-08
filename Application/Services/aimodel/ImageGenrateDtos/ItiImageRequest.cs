namespace V02.DTOs.ImageGenrate
{
    public class ItiImageRequest
    {
        public string model_id { get; set; } = "amazon.titan-image-generator-v2:0";
        public string prompt { get; set; } 
        public string task_type { get; set; } = "TEXT_IMAGE"; 
        public ImageGenerationConfig image_generation_config { get; set; } = new ImageGenerationConfig();
    }
    public class ImageGenerationConfig
    {
        public int number_of_images { get; set; } = 1;
        public string quality { get; set; } = "premium"; 
        public string height { get; set; } = "1024";
        public string width { get; set; } = "1024";
    }

    
    public class ItiImageResponse
    {
        public string generated_image { get; set; }
    }
}
