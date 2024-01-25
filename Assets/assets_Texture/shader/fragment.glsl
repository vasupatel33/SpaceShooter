#ifdef GL_ES
#define LOWP lowp
precision mediump float;
#else
#define LOWP
#endif
varying vec4 v_color;
varying vec2 v_texCoords;
uniform sampler2D u_texture;

// 目前在这个示例中不使用!
//uniform float time;
// 当然目标渲染的宽度
uniform float rt_w;
// 当前渲染目标的高度
uniform float rt_h;
// 漩涡特效参数
uniform float radius;
uniform vec2 center;

vec4 PostFX(sampler2D tex, vec2 uv, float time)
{
    float angle = 0.8;
    vec2 texSize = vec2(rt_w, rt_h);
    vec2 tc = uv * texSize;
    tc =tc- center;
    float dist = length(tc);
    if (dist < radius)
    {
        float percent = (radius - dist) / radius;
        float theta = percent * percent * angle * 8.0;
        float s = sin(theta);
        float c = cos(theta);
        tc = vec2(dot(tc, vec2(c, -s)), dot(tc, vec2(s, c)));
    }
    tc =tc+ center;
    vec4 color = texture2D(tex, tc / texSize).rgba;
    return color;
}

void main()
{
    //gl_FragColor = v_color * texture2D(u_texture, v_texCoords);

    vec2 uv = v_texCoords;
    gl_FragColor = PostFX(u_texture, uv, 0.0);
}
