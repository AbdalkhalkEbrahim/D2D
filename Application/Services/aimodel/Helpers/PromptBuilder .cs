using Domain.Entities.Chats.AiModel;
using System.Collections.Generic;

namespace V02
{
    public class PromptBuilder : IPromptBuilder
    {
        public string Build(DesignState state)
        {
            var parts = new List<string>();

            // 1. القوام الأساسي، نوع التصميم، والجنس المستهدف (e.g., Slim Fit Dress for Women)
            string baseDesign = $"{state.Fit} {state.Type}".Trim();
            if (!string.IsNullOrWhiteSpace(state.TargetGender))
            {
                baseDesign += $" for {state.TargetGender}";
            }
            parts.Add(baseDesign);

            // 2. فئة الستايل العام
            if (!string.IsNullOrWhiteSpace(state.StyleCategory))
                parts.Add($"{state.StyleCategory} fashion style");

            // 3. تفاصيل الـ Cut (الأكمام، فتحة الرقبة، والطول)
            if (!string.IsNullOrWhiteSpace(state.Sleeve))
                parts.Add($"{state.Sleeve} sleeves");

            if (!string.IsNullOrWhiteSpace(state.Neckline))
                parts.Add($"{state.Neckline} neckline");

            if (!string.IsNullOrWhiteSpace(state.Length))
                parts.Add($"{state.Length} length");

            // 4. الألوان (الأساسية والثانوية)
            if (!string.IsNullOrWhiteSpace(state.ColorPrimary))
                parts.Add($"dominant {state.ColorPrimary} color");

            if (!string.IsNullOrWhiteSpace(state.Secondary))
                parts.Add($"with {state.Secondary} secondary accents");

            // 5. تفاصيل خامة القماش (الثقل، النوع، والملمس)
            string fabricDescription = "";
            if (!string.IsNullOrWhiteSpace(state.FabricWeight))
                fabricDescription += $"{state.FabricWeight} ";

            if (!string.IsNullOrWhiteSpace(state.Material))
                fabricDescription += $"{state.Material} fabric";

            if (!string.IsNullOrWhiteSpace(state.Texture))
                fabricDescription += $" with {state.Texture} texture";

            if (!string.IsNullOrWhiteSpace(fabricDescription.Trim()))
                parts.Add(fabricDescription.Trim());

            // 6. الطابع أو الفكرة العامة للتصميم
            if (!string.IsNullOrWhiteSpace(state.Theme))
                parts.Add($"{state.Theme} theme");

            // 7. مصفوفة التفاصيل الإضافية والدقيقة (Details Array)
            if (state.Details != null && state.Details.Count > 0)
            {
                parts.AddRange(state.Details);
            }

            // 8. حقن توجيه صريح لموديل الـ Inpaint في حالة التعديل الجزئي
            if (!string.IsNullOrEmpty(state.OriginalImageReference) && !string.IsNullOrEmpty(state.InpaintMaskPrompt))
            {
                parts.Add($"specifically modifying and transforming the {state.ModificationArea} area");
            }

            // 9. زوايا العرض والخلفية الموحدة لضمان ثبات الاستوديو التخيلي
            parts.Add(!string.IsNullOrWhiteSpace(state.Views) ? state.Views : "photorealistic fashion studio showcase view");
            parts.Add(!string.IsNullOrWhiteSpace(state.Background) ? state.Background : "clean solid minimalist background");

            // دمج كل الأجزاء معاً لتسليمها لـ Nova Canvas أو Stable Inpaint كـ Prompt جاهز
            return string.Join(", ", parts);
        }
    }
}