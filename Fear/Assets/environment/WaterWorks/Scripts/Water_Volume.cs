#pragma warning disable CS0618 // obsolete
#pragma warning disable CS0672 // overrides obsolete


using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Water_Volume : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private Material _material;

        private RTHandle sourceHandle;
        private RTHandle tempRenderTarget;
        private RTHandle tempRenderTarget2;

        public CustomRenderPass(Material mat)
        {
            _material = mat;
        }

        public void SetSource(RTHandle handle)
        {
            sourceHandle = handle;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            tempRenderTarget = RTHandles.Alloc(cameraTextureDescriptor, name: "_TemporaryColourTexture");
            tempRenderTarget2 = RTHandles.Alloc(cameraTextureDescriptor, name: "_TemporaryDepthTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType != CameraType.Reflection)
            {
                CommandBuffer commandBuffer = CommandBufferPool.Get("WaterVolumePass");

                Blit(commandBuffer, sourceHandle, tempRenderTarget, _material);
                Blit(commandBuffer, tempRenderTarget, sourceHandle);

                context.ExecuteCommandBuffer(commandBuffer);
                CommandBufferPool.Release(commandBuffer);
            }
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            RTHandles.Release(tempRenderTarget);
            RTHandles.Release(tempRenderTarget2);
        }
    }

    [System.Serializable]
    public class _Settings
    {
        public Material material = null;
        public RenderPassEvent renderPass = RenderPassEvent.AfterRenderingSkybox;
    }

    public _Settings settings = new _Settings();
    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        if (settings.material == null)
        {
            settings.material = (Material)Resources.Load("Water_Volume");
        }

        m_ScriptablePass = new CustomRenderPass(settings.material);
        m_ScriptablePass.renderPassEvent = settings.renderPass;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        m_ScriptablePass.SetSource(renderer.cameraColorTargetHandle);
        renderer.EnqueuePass(m_ScriptablePass);
    }
}
