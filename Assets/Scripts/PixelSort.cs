using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PixelSortRenderer), PostProcessEvent.AfterStack, "Custom/PixelSort", true)]
public sealed class PixelSort : PostProcessEffectSettings {
	[Range(0f, 1f), Tooltip("Pixel Sort.")]
	public FloatParameter blend = new FloatParameter { value = 0f };
}

public sealed class PixelSortRenderer : PostProcessEffectRenderer<PixelSort> {
	public override void Render(PostProcessRenderContext context) {
		var sheet = context.propertySheets.Get(Shader.Find("Custom/PixelSort"));
		sheet.properties.SetFloat("_Blend", settings.blend);
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}
