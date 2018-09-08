using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(WarpRenderer), PostProcessEvent.AfterStack, "Custom/Warp", true)]
public sealed class Warp : PostProcessEffectSettings {
	[Range(0f, 1f), Tooltip("Warp.")]
	public FloatParameter warp = new FloatParameter { value = 0f };
}

public sealed class WarpRenderer : PostProcessEffectRenderer<Warp> {
	public override void Render(PostProcessRenderContext context) {
		var sheet = context.propertySheets.Get(Shader.Find("Custom/Warp"));
        settings.warp.Override(GlobalState.I.Warp);
        sheet.properties.SetFloat("_Blend", Easing.EaseInCubic(0,1,GlobalState.I.Warp));
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}
