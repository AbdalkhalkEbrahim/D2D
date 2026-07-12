using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Chats.AiModel
{
    public class DesignState
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = "";
        public string Fit { get; set; } = "";
        public string Sleeve { get; set; } = "";
        public string Length { get; set; } = "";
        public string? Neckline { get; set; } = ""; // 🆕 إضافي
        public string? StyleCategory { get; set; } = ""; // 🆕 كاجوال، رسمي، إلخ
        public string? TargetGender { get; set; } = ""; // 🆕 رجال، نساء، يونيسكس
        public string ColorPrimary { get; set; } = "";
        public string Secondary { get; set; } = "";
        public string Material { get; set; } = "";
        public string? FabricWeight { get; set; } = ""; // 🆕 صيفي خفيف، شتوي ثقيل
        public string Texture { get; set; } = "";
        public string Theme { get; set; } = "";
        public List<string> Details { get; set; } = new();
        public string Views { get; set; } = "";
        public string Background { get; set; } = "";

        // 🎯 حقول مخصصة للتحكم في الـ Inpaint والتعديل
        public string? ModificationArea { get; set; } = ""; // الجزء المطلوب تعديله (مثلاً: الياقة، الأكمام)
        public string? InpaintMaskPrompt { get; set; } = ""; // الوصف النصي للمنطقة لعزلها بالموديل
        public string? OriginalImageReference { get; set; } = ""; // رابط الصورة الأصلية
    }
}