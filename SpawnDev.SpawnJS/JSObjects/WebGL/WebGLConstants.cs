// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// WebGL and WebGL2 specification constants (GLenum values).
    /// All values are typed as uint to match the GLenum specification type.
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/WebGL_API/Constants"/>
    /// </summary>
    public static class GL
    {
        #region Clearing buffers
        /// <summary>
        /// Clearing buffers. Value 0x00000100.
        /// </summary>
        public const uint DEPTH_BUFFER_BIT = 0x00000100;
        /// <summary>
        /// Clearing buffers. Value 0x00000400.
        /// </summary>
        public const uint STENCIL_BUFFER_BIT = 0x00000400;
        /// <summary>
        /// Clearing buffers. Value 0x00004000.
        /// </summary>
        public const uint COLOR_BUFFER_BIT = 0x00004000;
        #endregion

        #region Rendering primitives
        /// <summary>
        /// Rendering primitives. Value 0x0000.
        /// </summary>
        public const uint POINTS = 0x0000;
        /// <summary>
        /// Rendering primitives. Value 0x0001.
        /// </summary>
        public const uint LINES = 0x0001;
        /// <summary>
        /// Rendering primitives. Value 0x0002.
        /// </summary>
        public const uint LINE_LOOP = 0x0002;
        /// <summary>
        /// Rendering primitives. Value 0x0003.
        /// </summary>
        public const uint LINE_STRIP = 0x0003;
        /// <summary>
        /// Rendering primitives. Value 0x0004.
        /// </summary>
        public const uint TRIANGLES = 0x0004;
        /// <summary>
        /// Rendering primitives. Value 0x0005.
        /// </summary>
        public const uint TRIANGLE_STRIP = 0x0005;
        /// <summary>
        /// Rendering primitives. Value 0x0006.
        /// </summary>
        public const uint TRIANGLE_FAN = 0x0006;
        #endregion

        #region Blending modes
        /// <summary>
        /// Blending modes. Value 0.
        /// </summary>
        public const uint ZERO = 0;
        /// <summary>
        /// Blending modes. Value 1.
        /// </summary>
        public const uint ONE = 1;
        /// <summary>
        /// Blending modes. Value 0x0300.
        /// </summary>
        public const uint SRC_COLOR = 0x0300;
        /// <summary>
        /// Blending modes. Value 0x0301.
        /// </summary>
        public const uint ONE_MINUS_SRC_COLOR = 0x0301;
        /// <summary>
        /// Blending modes. Value 0x0302.
        /// </summary>
        public const uint SRC_ALPHA = 0x0302;
        /// <summary>
        /// Blending modes. Value 0x0303.
        /// </summary>
        public const uint ONE_MINUS_SRC_ALPHA = 0x0303;
        /// <summary>
        /// Blending modes. Value 0x0304.
        /// </summary>
        public const uint DST_ALPHA = 0x0304;
        /// <summary>
        /// Blending modes. Value 0x0305.
        /// </summary>
        public const uint ONE_MINUS_DST_ALPHA = 0x0305;
        /// <summary>
        /// Blending modes. Value 0x0306.
        /// </summary>
        public const uint DST_COLOR = 0x0306;
        /// <summary>
        /// Blending modes. Value 0x0307.
        /// </summary>
        public const uint ONE_MINUS_DST_COLOR = 0x0307;
        /// <summary>
        /// Blending modes. Value 0x0308.
        /// </summary>
        public const uint SRC_ALPHA_SATURATE = 0x0308;
        /// <summary>
        /// Blending modes. Value 0x8001.
        /// </summary>
        public const uint CONSTANT_COLOR = 0x8001;
        /// <summary>
        /// Blending modes. Value 0x8002.
        /// </summary>
        public const uint ONE_MINUS_CONSTANT_COLOR = 0x8002;
        /// <summary>
        /// Blending modes. Value 0x8003.
        /// </summary>
        public const uint CONSTANT_ALPHA = 0x8003;
        /// <summary>
        /// Blending modes. Value 0x8004.
        /// </summary>
        public const uint ONE_MINUS_CONSTANT_ALPHA = 0x8004;
        #endregion

        #region Blending equations
        /// <summary>
        /// Blending equations. Value 0x8006.
        /// </summary>
        public const uint FUNC_ADD = 0x8006;
        /// <summary>
        /// Blending equations. Value 0x800A.
        /// </summary>
        public const uint FUNC_SUBTRACT = 0x800A;
        /// <summary>
        /// Blending equations. Value 0x800B.
        /// </summary>
        public const uint FUNC_REVERSE_SUBTRACT = 0x800B;
        /// <summary>
        /// Blending equations. Value 0x8009.
        /// </summary>
        public const uint BLEND_EQUATION = 0x8009;
        /// <summary>
        /// Blending equations. Value 0x8009.
        /// </summary>
        public const uint BLEND_EQUATION_RGB = 0x8009;
        /// <summary>
        /// Blending equations. Value 0x883D.
        /// </summary>
        public const uint BLEND_EQUATION_ALPHA = 0x883D;
        /// <summary>
        /// Blending equations. Value 0x80C8.
        /// </summary>
        public const uint BLEND_DST_RGB = 0x80C8;
        /// <summary>
        /// Blending equations. Value 0x80C9.
        /// </summary>
        public const uint BLEND_SRC_RGB = 0x80C9;
        /// <summary>
        /// Blending equations. Value 0x80CA.
        /// </summary>
        public const uint BLEND_DST_ALPHA = 0x80CA;
        /// <summary>
        /// Blending equations. Value 0x80CB.
        /// </summary>
        public const uint BLEND_SRC_ALPHA = 0x80CB;
        /// <summary>
        /// Blending equations. Value 0x8005.
        /// </summary>
        public const uint BLEND_COLOR = 0x8005;
        #endregion

        #region Buffers
        /// <summary>
        /// Buffers. Value 0x8892.
        /// </summary>
        public const uint ARRAY_BUFFER = 0x8892;
        /// <summary>
        /// Buffers. Value 0x8893.
        /// </summary>
        public const uint ELEMENT_ARRAY_BUFFER = 0x8893;
        /// <summary>
        /// Buffers. Value 0x8894.
        /// </summary>
        public const uint ARRAY_BUFFER_BINDING = 0x8894;
        /// <summary>
        /// Buffers. Value 0x8895.
        /// </summary>
        public const uint ELEMENT_ARRAY_BUFFER_BINDING = 0x8895;
        /// <summary>
        /// Buffers. Value 0x88E0.
        /// </summary>
        public const uint STREAM_DRAW = 0x88E0;
        /// <summary>
        /// Buffers. Value 0x88E4.
        /// </summary>
        public const uint STATIC_DRAW = 0x88E4;
        /// <summary>
        /// Buffers. Value 0x88E8.
        /// </summary>
        public const uint DYNAMIC_DRAW = 0x88E8;
        /// <summary>
        /// Buffers. Value 0x8764.
        /// </summary>
        public const uint BUFFER_SIZE = 0x8764;
        /// <summary>
        /// Buffers. Value 0x8765.
        /// </summary>
        public const uint BUFFER_USAGE = 0x8765;
        #endregion

        #region Vertex attributes
        /// <summary>
        /// Vertex attributes. Value 0x8626.
        /// </summary>
        public const uint CURRENT_VERTEX_ATTRIB = 0x8626;
        /// <summary>
        /// Vertex attributes. Value 0x8622.
        /// </summary>
        public const uint VERTEX_ATTRIB_ARRAY_ENABLED = 0x8622;
        /// <summary>
        /// Vertex attributes. Value 0x8623.
        /// </summary>
        public const uint VERTEX_ATTRIB_ARRAY_SIZE = 0x8623;
        /// <summary>
        /// Vertex attributes. Value 0x8624.
        /// </summary>
        public const uint VERTEX_ATTRIB_ARRAY_STRIDE = 0x8624;
        /// <summary>
        /// Vertex attributes. Value 0x8625.
        /// </summary>
        public const uint VERTEX_ATTRIB_ARRAY_TYPE = 0x8625;
        /// <summary>
        /// Vertex attributes. Value 0x886A.
        /// </summary>
        public const uint VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x886A;
        /// <summary>
        /// Vertex attributes. Value 0x8645.
        /// </summary>
        public const uint VERTEX_ATTRIB_ARRAY_POINTER = 0x8645;
        /// <summary>
        /// Vertex attributes. Value 0x889F.
        /// </summary>
        public const uint VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x889F;
        #endregion

        #region Culling
        /// <summary>
        /// Culling. Value 0x0B44.
        /// </summary>
        public const uint CULL_FACE = 0x0B44;
        /// <summary>
        /// Culling. Value 0x0404.
        /// </summary>
        public const uint FRONT = 0x0404;
        /// <summary>
        /// Culling. Value 0x0405.
        /// </summary>
        public const uint BACK = 0x0405;
        /// <summary>
        /// Culling. Value 0x0408.
        /// </summary>
        public const uint FRONT_AND_BACK = 0x0408;
        #endregion

        #region Enabling and disabling
        /// <summary>
        /// Enabling and disabling. Value 0x0BE2.
        /// </summary>
        public const uint BLEND = 0x0BE2;
        /// <summary>
        /// Enabling and disabling. Value 0x0B71.
        /// </summary>
        public const uint DEPTH_TEST = 0x0B71;
        /// <summary>
        /// Enabling and disabling. Value 0x0BD0.
        /// </summary>
        public const uint DITHER = 0x0BD0;
        /// <summary>
        /// Enabling and disabling. Value 0x8037.
        /// </summary>
        public const uint POLYGON_OFFSET_FILL = 0x8037;
        /// <summary>
        /// Enabling and disabling. Value 0x809E.
        /// </summary>
        public const uint SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
        /// <summary>
        /// Enabling and disabling. Value 0x80A0.
        /// </summary>
        public const uint SAMPLE_COVERAGE = 0x80A0;
        /// <summary>
        /// Enabling and disabling. Value 0x0C11.
        /// </summary>
        public const uint SCISSOR_TEST = 0x0C11;
        /// <summary>
        /// Enabling and disabling. Value 0x0B90.
        /// </summary>
        public const uint STENCIL_TEST = 0x0B90;
        #endregion

        #region Errors
        /// <summary>
        /// Errors. Value 0.
        /// </summary>
        public const uint NO_ERROR = 0;
        /// <summary>
        /// Errors. Value 0x0500.
        /// </summary>
        public const uint INVALID_ENUM = 0x0500;
        /// <summary>
        /// Errors. Value 0x0501.
        /// </summary>
        public const uint INVALID_VALUE = 0x0501;
        /// <summary>
        /// Errors. Value 0x0502.
        /// </summary>
        public const uint INVALID_OPERATION = 0x0502;
        /// <summary>
        /// Errors. Value 0x0505.
        /// </summary>
        public const uint OUT_OF_MEMORY = 0x0505;
        /// <summary>
        /// Errors. Value 0x9242.
        /// </summary>
        public const uint CONTEXT_LOST_WEBGL = 0x9242;
        #endregion

        #region Front face directions
        /// <summary>
        /// Front face directions. Value 0x0900.
        /// </summary>
        public const uint CW = 0x0900;
        /// <summary>
        /// Front face directions. Value 0x0901.
        /// </summary>
        public const uint CCW = 0x0901;
        #endregion

        #region Hints
        /// <summary>
        /// Hints. Value 0x1100.
        /// </summary>
        public const uint DONT_CARE = 0x1100;
        /// <summary>
        /// Hints. Value 0x1101.
        /// </summary>
        public const uint FASTEST = 0x1101;
        /// <summary>
        /// Hints. Value 0x1102.
        /// </summary>
        public const uint NICEST = 0x1102;
        /// <summary>
        /// Hints. Value 0x8192.
        /// </summary>
        public const uint GENERATE_MIPMAP_HINT = 0x8192;
        #endregion

        #region Data types
        /// <summary>
        /// Data types. Value 0x1400.
        /// </summary>
        public const uint BYTE = 0x1400;
        /// <summary>
        /// Data types. Value 0x1401.
        /// </summary>
        public const uint UNSIGNED_BYTE = 0x1401;
        /// <summary>
        /// Data types. Value 0x1402.
        /// </summary>
        public const uint SHORT = 0x1402;
        /// <summary>
        /// Data types. Value 0x1403.
        /// </summary>
        public const uint UNSIGNED_SHORT = 0x1403;
        /// <summary>
        /// Data types. Value 0x1404.
        /// </summary>
        public const uint INT = 0x1404;
        /// <summary>
        /// Data types. Value 0x1405.
        /// </summary>
        public const uint UNSIGNED_INT = 0x1405;
        /// <summary>
        /// Data types. Value 0x1406.
        /// </summary>
        public const uint FLOAT = 0x1406;
        #endregion

        #region Pixel formats
        /// <summary>
        /// Pixel formats. Value 0x1902.
        /// </summary>
        public const uint DEPTH_COMPONENT = 0x1902;
        /// <summary>
        /// Pixel formats. Value 0x1906.
        /// </summary>
        public const uint ALPHA = 0x1906;
        /// <summary>
        /// Pixel formats. Value 0x1907.
        /// </summary>
        public const uint RGB = 0x1907;
        /// <summary>
        /// Pixel formats. Value 0x1908.
        /// </summary>
        public const uint RGBA = 0x1908;
        /// <summary>
        /// Pixel formats. Value 0x1909.
        /// </summary>
        public const uint LUMINANCE = 0x1909;
        /// <summary>
        /// Pixel formats. Value 0x190A.
        /// </summary>
        public const uint LUMINANCE_ALPHA = 0x190A;
        #endregion

        #region Pixel types
        /// <summary>
        /// Pixel types. Value 0x8033.
        /// </summary>
        public const uint UNSIGNED_SHORT_4_4_4_4 = 0x8033;
        /// <summary>
        /// Pixel types. Value 0x8034.
        /// </summary>
        public const uint UNSIGNED_SHORT_5_5_5_1 = 0x8034;
        /// <summary>
        /// Pixel types. Value 0x8363.
        /// </summary>
        public const uint UNSIGNED_SHORT_5_6_5 = 0x8363;
        #endregion

        #region Shaders
        /// <summary>
        /// Shaders. Value 0x8B30.
        /// </summary>
        public const uint FRAGMENT_SHADER = 0x8B30;
        /// <summary>
        /// Shaders. Value 0x8B31.
        /// </summary>
        public const uint VERTEX_SHADER = 0x8B31;
        /// <summary>
        /// Shaders. Value 0x8B81.
        /// </summary>
        public const uint COMPILE_STATUS = 0x8B81;
        /// <summary>
        /// Shaders. Value 0x8B80.
        /// </summary>
        public const uint DELETE_STATUS = 0x8B80;
        /// <summary>
        /// Shaders. Value 0x8B82.
        /// </summary>
        public const uint LINK_STATUS = 0x8B82;
        /// <summary>
        /// Shaders. Value 0x8B83.
        /// </summary>
        public const uint VALIDATE_STATUS = 0x8B83;
        /// <summary>
        /// Shaders. Value 0x8B85.
        /// </summary>
        public const uint ATTACHED_SHADERS = 0x8B85;
        /// <summary>
        /// Shaders. Value 0x8B89.
        /// </summary>
        public const uint ACTIVE_ATTRIBUTES = 0x8B89;
        /// <summary>
        /// Shaders. Value 0x8B86.
        /// </summary>
        public const uint ACTIVE_UNIFORMS = 0x8B86;
        /// <summary>
        /// Shaders. Value 0x8869.
        /// </summary>
        public const uint MAX_VERTEX_ATTRIBS = 0x8869;
        /// <summary>
        /// Shaders. Value 0x8DFB.
        /// </summary>
        public const uint MAX_VERTEX_UNIFORM_VECTORS = 0x8DFB;
        /// <summary>
        /// Shaders. Value 0x8DFC.
        /// </summary>
        public const uint MAX_VARYING_VECTORS = 0x8DFC;
        /// <summary>
        /// Shaders. Value 0x8B4D.
        /// </summary>
        public const uint MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x8B4D;
        /// <summary>
        /// Shaders. Value 0x8B4C.
        /// </summary>
        public const uint MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x8B4C;
        /// <summary>
        /// Shaders. Value 0x8872.
        /// </summary>
        public const uint MAX_TEXTURE_IMAGE_UNITS = 0x8872;
        /// <summary>
        /// Shaders. Value 0x8DFD.
        /// </summary>
        public const uint MAX_FRAGMENT_UNIFORM_VECTORS = 0x8DFD;
        /// <summary>
        /// Shaders. Value 0x8B4F.
        /// </summary>
        public const uint SHADER_TYPE = 0x8B4F;
        /// <summary>
        /// Shaders. Value 0x8B8C.
        /// </summary>
        public const uint SHADING_LANGUAGE_VERSION = 0x8B8C;
        /// <summary>
        /// Shaders. Value 0x8B8D.
        /// </summary>
        public const uint CURRENT_PROGRAM = 0x8B8D;
        #endregion

        #region Depth or stencil tests
        /// <summary>
        /// Depth or stencil tests. Value 0x0200.
        /// </summary>
        public const uint NEVER = 0x0200;
        /// <summary>
        /// Depth or stencil tests. Value 0x0201.
        /// </summary>
        public const uint LESS = 0x0201;
        /// <summary>
        /// Depth or stencil tests. Value 0x0202.
        /// </summary>
        public const uint EQUAL = 0x0202;
        /// <summary>
        /// Depth or stencil tests. Value 0x0203.
        /// </summary>
        public const uint LEQUAL = 0x0203;
        /// <summary>
        /// Depth or stencil tests. Value 0x0204.
        /// </summary>
        public const uint GREATER = 0x0204;
        /// <summary>
        /// Depth or stencil tests. Value 0x0205.
        /// </summary>
        public const uint NOTEQUAL = 0x0205;
        /// <summary>
        /// Depth or stencil tests. Value 0x0206.
        /// </summary>
        public const uint GEQUAL = 0x0206;
        /// <summary>
        /// Depth or stencil tests. Value 0x0207.
        /// </summary>
        public const uint ALWAYS = 0x0207;
        #endregion

        #region Stencil actions
        /// <summary>
        /// Stencil actions. Value 0x1E00.
        /// </summary>
        public const uint KEEP = 0x1E00;
        /// <summary>
        /// Stencil actions. Value 0x1E01.
        /// </summary>
        public const uint REPLACE = 0x1E01;
        /// <summary>
        /// Stencil actions. Value 0x1E02.
        /// </summary>
        public const uint INCR = 0x1E02;
        /// <summary>
        /// Stencil actions. Value 0x1E03.
        /// </summary>
        public const uint DECR = 0x1E03;
        /// <summary>
        /// Stencil actions. Value 0x150A.
        /// </summary>
        public const uint INVERT = 0x150A;
        /// <summary>
        /// Stencil actions. Value 0x8507.
        /// </summary>
        public const uint INCR_WRAP = 0x8507;
        /// <summary>
        /// Stencil actions. Value 0x8508.
        /// </summary>
        public const uint DECR_WRAP = 0x8508;
        #endregion

        #region Textures
        /// <summary>
        /// Textures. Value 0x2600.
        /// </summary>
        public const uint NEAREST = 0x2600;
        /// <summary>
        /// Textures. Value 0x2601.
        /// </summary>
        public const uint LINEAR = 0x2601;
        /// <summary>
        /// Textures. Value 0x2700.
        /// </summary>
        public const uint NEAREST_MIPMAP_NEAREST = 0x2700;
        /// <summary>
        /// Textures. Value 0x2701.
        /// </summary>
        public const uint LINEAR_MIPMAP_NEAREST = 0x2701;
        /// <summary>
        /// Textures. Value 0x2702.
        /// </summary>
        public const uint NEAREST_MIPMAP_LINEAR = 0x2702;
        /// <summary>
        /// Textures. Value 0x2703.
        /// </summary>
        public const uint LINEAR_MIPMAP_LINEAR = 0x2703;
        /// <summary>
        /// Textures. Value 0x2800.
        /// </summary>
        public const uint TEXTURE_MAG_FILTER = 0x2800;
        /// <summary>
        /// Textures. Value 0x2801.
        /// </summary>
        public const uint TEXTURE_MIN_FILTER = 0x2801;
        /// <summary>
        /// Textures. Value 0x2802.
        /// </summary>
        public const uint TEXTURE_WRAP_S = 0x2802;
        /// <summary>
        /// Textures. Value 0x2803.
        /// </summary>
        public const uint TEXTURE_WRAP_T = 0x2803;
        /// <summary>
        /// Textures. Value 0x0DE1.
        /// </summary>
        public const uint TEXTURE_2D = 0x0DE1;
        /// <summary>
        /// Textures. Value 0x1702.
        /// </summary>
        public const uint TEXTURE = 0x1702;
        /// <summary>
        /// Textures. Value 0x8513.
        /// </summary>
        public const uint TEXTURE_CUBE_MAP = 0x8513;
        /// <summary>
        /// Textures. Value 0x8514.
        /// </summary>
        public const uint TEXTURE_BINDING_CUBE_MAP = 0x8514;
        /// <summary>
        /// Textures. Value 0x8515.
        /// </summary>
        public const uint TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
        /// <summary>
        /// Textures. Value 0x8516.
        /// </summary>
        public const uint TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
        /// <summary>
        /// Textures. Value 0x8517.
        /// </summary>
        public const uint TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
        /// <summary>
        /// Textures. Value 0x8518.
        /// </summary>
        public const uint TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
        /// <summary>
        /// Textures. Value 0x8519.
        /// </summary>
        public const uint TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
        /// <summary>
        /// Textures. Value 0x851A.
        /// </summary>
        public const uint TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
        /// <summary>
        /// Textures. Value 0x851C.
        /// </summary>
        public const uint MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;
        /// <summary>
        /// Textures. Value 0x84E0.
        /// </summary>
        public const uint ACTIVE_TEXTURE = 0x84E0;
        /// <summary>
        /// Textures. Value 0x2901.
        /// </summary>
        public const uint REPEAT = 0x2901;
        /// <summary>
        /// Textures. Value 0x812F.
        /// </summary>
        public const uint CLAMP_TO_EDGE = 0x812F;
        /// <summary>
        /// Textures. Value 0x8370.
        /// </summary>
        public const uint MIRRORED_REPEAT = 0x8370;
        #endregion

        #region Texture units
        /// <summary>
        /// Texture units. Value 0x84C0.
        /// </summary>
        public const uint TEXTURE0 = 0x84C0;
        /// <summary>
        /// Texture units. Value 0x84C1.
        /// </summary>
        public const uint TEXTURE1 = 0x84C1;
        /// <summary>
        /// Texture units. Value 0x84C2.
        /// </summary>
        public const uint TEXTURE2 = 0x84C2;
        /// <summary>
        /// Texture units. Value 0x84C3.
        /// </summary>
        public const uint TEXTURE3 = 0x84C3;
        /// <summary>
        /// Texture units. Value 0x84C4.
        /// </summary>
        public const uint TEXTURE4 = 0x84C4;
        /// <summary>
        /// Texture units. Value 0x84C5.
        /// </summary>
        public const uint TEXTURE5 = 0x84C5;
        /// <summary>
        /// Texture units. Value 0x84C6.
        /// </summary>
        public const uint TEXTURE6 = 0x84C6;
        /// <summary>
        /// Texture units. Value 0x84C7.
        /// </summary>
        public const uint TEXTURE7 = 0x84C7;
        /// <summary>
        /// Texture units. Value 0x84C8.
        /// </summary>
        public const uint TEXTURE8 = 0x84C8;
        /// <summary>
        /// Texture units. Value 0x84C9.
        /// </summary>
        public const uint TEXTURE9 = 0x84C9;
        /// <summary>
        /// Texture units. Value 0x84CA.
        /// </summary>
        public const uint TEXTURE10 = 0x84CA;
        /// <summary>
        /// Texture units. Value 0x84CB.
        /// </summary>
        public const uint TEXTURE11 = 0x84CB;
        /// <summary>
        /// Texture units. Value 0x84CC.
        /// </summary>
        public const uint TEXTURE12 = 0x84CC;
        /// <summary>
        /// Texture units. Value 0x84CD.
        /// </summary>
        public const uint TEXTURE13 = 0x84CD;
        /// <summary>
        /// Texture units. Value 0x84CE.
        /// </summary>
        public const uint TEXTURE14 = 0x84CE;
        /// <summary>
        /// Texture units. Value 0x84CF.
        /// </summary>
        public const uint TEXTURE15 = 0x84CF;
        /// <summary>
        /// Texture units. Value 0x84D0.
        /// </summary>
        public const uint TEXTURE16 = 0x84D0;
        /// <summary>
        /// Texture units. Value 0x84D1.
        /// </summary>
        public const uint TEXTURE17 = 0x84D1;
        /// <summary>
        /// Texture units. Value 0x84D2.
        /// </summary>
        public const uint TEXTURE18 = 0x84D2;
        /// <summary>
        /// Texture units. Value 0x84D3.
        /// </summary>
        public const uint TEXTURE19 = 0x84D3;
        /// <summary>
        /// Texture units. Value 0x84D4.
        /// </summary>
        public const uint TEXTURE20 = 0x84D4;
        /// <summary>
        /// Texture units. Value 0x84D5.
        /// </summary>
        public const uint TEXTURE21 = 0x84D5;
        /// <summary>
        /// Texture units. Value 0x84D6.
        /// </summary>
        public const uint TEXTURE22 = 0x84D6;
        /// <summary>
        /// Texture units. Value 0x84D7.
        /// </summary>
        public const uint TEXTURE23 = 0x84D7;
        /// <summary>
        /// Texture units. Value 0x84D8.
        /// </summary>
        public const uint TEXTURE24 = 0x84D8;
        /// <summary>
        /// Texture units. Value 0x84D9.
        /// </summary>
        public const uint TEXTURE25 = 0x84D9;
        /// <summary>
        /// Texture units. Value 0x84DA.
        /// </summary>
        public const uint TEXTURE26 = 0x84DA;
        /// <summary>
        /// Texture units. Value 0x84DB.
        /// </summary>
        public const uint TEXTURE27 = 0x84DB;
        /// <summary>
        /// Texture units. Value 0x84DC.
        /// </summary>
        public const uint TEXTURE28 = 0x84DC;
        /// <summary>
        /// Texture units. Value 0x84DD.
        /// </summary>
        public const uint TEXTURE29 = 0x84DD;
        /// <summary>
        /// Texture units. Value 0x84DE.
        /// </summary>
        public const uint TEXTURE30 = 0x84DE;
        /// <summary>
        /// Texture units. Value 0x84DF.
        /// </summary>
        public const uint TEXTURE31 = 0x84DF;
        #endregion

        #region Uniform types
        /// <summary>
        /// Uniform types. Value 0x8B50.
        /// </summary>
        public const uint FLOAT_VEC2 = 0x8B50;
        /// <summary>
        /// Uniform types. Value 0x8B51.
        /// </summary>
        public const uint FLOAT_VEC3 = 0x8B51;
        /// <summary>
        /// Uniform types. Value 0x8B52.
        /// </summary>
        public const uint FLOAT_VEC4 = 0x8B52;
        /// <summary>
        /// Uniform types. Value 0x8B53.
        /// </summary>
        public const uint INT_VEC2 = 0x8B53;
        /// <summary>
        /// Uniform types. Value 0x8B54.
        /// </summary>
        public const uint INT_VEC3 = 0x8B54;
        /// <summary>
        /// Uniform types. Value 0x8B55.
        /// </summary>
        public const uint INT_VEC4 = 0x8B55;
        /// <summary>
        /// Uniform types. Value 0x8B56.
        /// </summary>
        public const uint BOOL = 0x8B56;
        /// <summary>
        /// Uniform types. Value 0x8B57.
        /// </summary>
        public const uint BOOL_VEC2 = 0x8B57;
        /// <summary>
        /// Uniform types. Value 0x8B58.
        /// </summary>
        public const uint BOOL_VEC3 = 0x8B58;
        /// <summary>
        /// Uniform types. Value 0x8B59.
        /// </summary>
        public const uint BOOL_VEC4 = 0x8B59;
        /// <summary>
        /// Uniform types. Value 0x8B5A.
        /// </summary>
        public const uint FLOAT_MAT2 = 0x8B5A;
        /// <summary>
        /// Uniform types. Value 0x8B5B.
        /// </summary>
        public const uint FLOAT_MAT3 = 0x8B5B;
        /// <summary>
        /// Uniform types. Value 0x8B5C.
        /// </summary>
        public const uint FLOAT_MAT4 = 0x8B5C;
        /// <summary>
        /// Uniform types. Value 0x8B5E.
        /// </summary>
        public const uint SAMPLER_2D = 0x8B5E;
        /// <summary>
        /// Uniform types. Value 0x8B60.
        /// </summary>
        public const uint SAMPLER_CUBE = 0x8B60;
        #endregion

        #region Shader precision-specified types
        /// <summary>
        /// Shader precision-specified types. Value 0x8DF0.
        /// </summary>
        public const uint LOW_FLOAT = 0x8DF0;
        /// <summary>
        /// Shader precision-specified types. Value 0x8DF1.
        /// </summary>
        public const uint MEDIUM_FLOAT = 0x8DF1;
        /// <summary>
        /// Shader precision-specified types. Value 0x8DF2.
        /// </summary>
        public const uint HIGH_FLOAT = 0x8DF2;
        /// <summary>
        /// Shader precision-specified types. Value 0x8DF3.
        /// </summary>
        public const uint LOW_INT = 0x8DF3;
        /// <summary>
        /// Shader precision-specified types. Value 0x8DF4.
        /// </summary>
        public const uint MEDIUM_INT = 0x8DF4;
        /// <summary>
        /// Shader precision-specified types. Value 0x8DF5.
        /// </summary>
        public const uint HIGH_INT = 0x8DF5;
        #endregion

        #region Framebuffers and renderbuffers
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D40.
        /// </summary>
        public const uint FRAMEBUFFER = 0x8D40;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D41.
        /// </summary>
        public const uint RENDERBUFFER = 0x8D41;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8056.
        /// </summary>
        public const uint RGBA4 = 0x8056;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8057.
        /// </summary>
        public const uint RGB5_A1 = 0x8057;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D62.
        /// </summary>
        public const uint RGB565 = 0x8D62;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x81A5.
        /// </summary>
        public const uint DEPTH_COMPONENT16 = 0x81A5;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D48.
        /// </summary>
        public const uint STENCIL_INDEX8 = 0x8D48;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x84F9.
        /// </summary>
        public const uint DEPTH_STENCIL = 0x84F9;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D42.
        /// </summary>
        public const uint RENDERBUFFER_WIDTH = 0x8D42;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D43.
        /// </summary>
        public const uint RENDERBUFFER_HEIGHT = 0x8D43;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D44.
        /// </summary>
        public const uint RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D50.
        /// </summary>
        public const uint RENDERBUFFER_RED_SIZE = 0x8D50;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D51.
        /// </summary>
        public const uint RENDERBUFFER_GREEN_SIZE = 0x8D51;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D52.
        /// </summary>
        public const uint RENDERBUFFER_BLUE_SIZE = 0x8D52;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D53.
        /// </summary>
        public const uint RENDERBUFFER_ALPHA_SIZE = 0x8D53;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D54.
        /// </summary>
        public const uint RENDERBUFFER_DEPTH_SIZE = 0x8D54;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D55.
        /// </summary>
        public const uint RENDERBUFFER_STENCIL_SIZE = 0x8D55;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CD0.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CD1.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CD2.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CD3.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CE0.
        /// </summary>
        public const uint COLOR_ATTACHMENT0 = 0x8CE0;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D00.
        /// </summary>
        public const uint DEPTH_ATTACHMENT = 0x8D00;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8D20.
        /// </summary>
        public const uint STENCIL_ATTACHMENT = 0x8D20;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x821A.
        /// </summary>
        public const uint DEPTH_STENCIL_ATTACHMENT = 0x821A;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0.
        /// </summary>
        public const uint NONE = 0;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CD5.
        /// </summary>
        public const uint FRAMEBUFFER_COMPLETE = 0x8CD5;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CD6.
        /// </summary>
        public const uint FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CD7.
        /// </summary>
        public const uint FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CD9.
        /// </summary>
        public const uint FRAMEBUFFER_INCOMPLETE_DIMENSIONS = 0x8CD9;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CDD.
        /// </summary>
        public const uint FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CA6.
        /// </summary>
        public const uint FRAMEBUFFER_BINDING = 0x8CA6;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x8CA7.
        /// </summary>
        public const uint RENDERBUFFER_BINDING = 0x8CA7;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x84E8.
        /// </summary>
        public const uint MAX_RENDERBUFFER_SIZE = 0x84E8;
        /// <summary>
        /// Framebuffers and renderbuffers. Value 0x0506.
        /// </summary>
        public const uint INVALID_FRAMEBUFFER_OPERATION = 0x0506;
        #endregion

        #region Getting GL parameter information
        /// <summary>
        /// Getting GL parameter information. Value 0x0B21.
        /// </summary>
        public const uint LINE_WIDTH = 0x0B21;
        /// <summary>
        /// Getting GL parameter information. Value 0x846D.
        /// </summary>
        public const uint ALIASED_POINT_SIZE_RANGE = 0x846D;
        /// <summary>
        /// Getting GL parameter information. Value 0x846E.
        /// </summary>
        public const uint ALIASED_LINE_WIDTH_RANGE = 0x846E;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B45.
        /// </summary>
        public const uint CULL_FACE_MODE = 0x0B45;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B46.
        /// </summary>
        public const uint FRONT_FACE = 0x0B46;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B70.
        /// </summary>
        public const uint DEPTH_RANGE = 0x0B70;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B72.
        /// </summary>
        public const uint DEPTH_WRITEMASK = 0x0B72;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B73.
        /// </summary>
        public const uint DEPTH_CLEAR_VALUE = 0x0B73;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B74.
        /// </summary>
        public const uint DEPTH_FUNC = 0x0B74;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B91.
        /// </summary>
        public const uint STENCIL_CLEAR_VALUE = 0x0B91;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B92.
        /// </summary>
        public const uint STENCIL_FUNC = 0x0B92;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B94.
        /// </summary>
        public const uint STENCIL_FAIL = 0x0B94;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B95.
        /// </summary>
        public const uint STENCIL_PASS_DEPTH_FAIL = 0x0B95;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B96.
        /// </summary>
        public const uint STENCIL_PASS_DEPTH_PASS = 0x0B96;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B97.
        /// </summary>
        public const uint STENCIL_REF = 0x0B97;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B93.
        /// </summary>
        public const uint STENCIL_VALUE_MASK = 0x0B93;
        /// <summary>
        /// Getting GL parameter information. Value 0x0B98.
        /// </summary>
        public const uint STENCIL_WRITEMASK = 0x0B98;
        /// <summary>
        /// Getting GL parameter information. Value 0x8800.
        /// </summary>
        public const uint STENCIL_BACK_FUNC = 0x8800;
        /// <summary>
        /// Getting GL parameter information. Value 0x8801.
        /// </summary>
        public const uint STENCIL_BACK_FAIL = 0x8801;
        /// <summary>
        /// Getting GL parameter information. Value 0x8802.
        /// </summary>
        public const uint STENCIL_BACK_PASS_DEPTH_FAIL = 0x8802;
        /// <summary>
        /// Getting GL parameter information. Value 0x8803.
        /// </summary>
        public const uint STENCIL_BACK_PASS_DEPTH_PASS = 0x8803;
        /// <summary>
        /// Getting GL parameter information. Value 0x8CA3.
        /// </summary>
        public const uint STENCIL_BACK_REF = 0x8CA3;
        /// <summary>
        /// Getting GL parameter information. Value 0x8CA4.
        /// </summary>
        public const uint STENCIL_BACK_VALUE_MASK = 0x8CA4;
        /// <summary>
        /// Getting GL parameter information. Value 0x8CA5.
        /// </summary>
        public const uint STENCIL_BACK_WRITEMASK = 0x8CA5;
        /// <summary>
        /// Getting GL parameter information. Value 0x0BA2.
        /// </summary>
        public const uint VIEWPORT = 0x0BA2;
        /// <summary>
        /// Getting GL parameter information. Value 0x0C10.
        /// </summary>
        public const uint SCISSOR_BOX = 0x0C10;
        /// <summary>
        /// Getting GL parameter information. Value 0x0C22.
        /// </summary>
        public const uint COLOR_CLEAR_VALUE = 0x0C22;
        /// <summary>
        /// Getting GL parameter information. Value 0x0C23.
        /// </summary>
        public const uint COLOR_WRITEMASK = 0x0C23;
        /// <summary>
        /// Getting GL parameter information. Value 0x0CF5.
        /// </summary>
        public const uint UNPACK_ALIGNMENT = 0x0CF5;
        /// <summary>
        /// Getting GL parameter information. Value 0x0D05.
        /// </summary>
        public const uint PACK_ALIGNMENT = 0x0D05;
        /// <summary>
        /// Getting GL parameter information. Value 0x0D33.
        /// </summary>
        public const uint MAX_TEXTURE_SIZE = 0x0D33;
        /// <summary>
        /// Getting GL parameter information. Value 0x0D3A.
        /// </summary>
        public const uint MAX_VIEWPORT_DIMS = 0x0D3A;
        /// <summary>
        /// Getting GL parameter information. Value 0x0D50.
        /// </summary>
        public const uint SUBPIXEL_BITS = 0x0D50;
        /// <summary>
        /// Getting GL parameter information. Value 0x0D52.
        /// </summary>
        public const uint RED_BITS = 0x0D52;
        /// <summary>
        /// Getting GL parameter information. Value 0x0D53.
        /// </summary>
        public const uint GREEN_BITS = 0x0D53;
        /// <summary>
        /// Getting GL parameter information. Value 0x0D54.
        /// </summary>
        public const uint BLUE_BITS = 0x0D54;
        /// <summary>
        /// Getting GL parameter information. Value 0x0D55.
        /// </summary>
        public const uint ALPHA_BITS = 0x0D55;
        /// <summary>
        /// Getting GL parameter information. Value 0x0D56.
        /// </summary>
        public const uint DEPTH_BITS = 0x0D56;
        /// <summary>
        /// Getting GL parameter information. Value 0x0D57.
        /// </summary>
        public const uint STENCIL_BITS = 0x0D57;
        /// <summary>
        /// Getting GL parameter information. Value 0x2A00.
        /// </summary>
        public const uint POLYGON_OFFSET_UNITS = 0x2A00;
        /// <summary>
        /// Getting GL parameter information. Value 0x8038.
        /// </summary>
        public const uint POLYGON_OFFSET_FACTOR = 0x8038;
        /// <summary>
        /// Getting GL parameter information. Value 0x8069.
        /// </summary>
        public const uint TEXTURE_BINDING_2D = 0x8069;
        /// <summary>
        /// Getting GL parameter information. Value 0x80A8.
        /// </summary>
        public const uint SAMPLE_BUFFERS = 0x80A8;
        /// <summary>
        /// Getting GL parameter information. Value 0x80A9.
        /// </summary>
        public const uint SAMPLES = 0x80A9;
        /// <summary>
        /// Getting GL parameter information. Value 0x80AA.
        /// </summary>
        public const uint SAMPLE_COVERAGE_VALUE = 0x80AA;
        /// <summary>
        /// Getting GL parameter information. Value 0x80AB.
        /// </summary>
        public const uint SAMPLE_COVERAGE_INVERT = 0x80AB;
        /// <summary>
        /// Getting GL parameter information. Value 0x86A3.
        /// </summary>
        public const uint COMPRESSED_TEXTURE_FORMATS = 0x86A3;
        /// <summary>
        /// Getting GL parameter information. Value 0x1F00.
        /// </summary>
        public const uint VENDOR = 0x1F00;
        /// <summary>
        /// Getting GL parameter information. Value 0x1F01.
        /// </summary>
        public const uint RENDERER = 0x1F01;
        /// <summary>
        /// Getting GL parameter information. Value 0x1F02.
        /// </summary>
        public const uint VERSION = 0x1F02;
        /// <summary>
        /// Getting GL parameter information. Value 0x8B9A.
        /// </summary>
        public const uint IMPLEMENTATION_COLOR_READ_TYPE = 0x8B9A;
        /// <summary>
        /// Getting GL parameter information. Value 0x8B9B.
        /// </summary>
        public const uint IMPLEMENTATION_COLOR_READ_FORMAT = 0x8B9B;
        /// <summary>
        /// Getting GL parameter information. Value 0x9244.
        /// </summary>
        public const uint BROWSER_DEFAULT_WEBGL = 0x9244;
        #endregion

        #region Pixel storage modes
        /// <summary>
        /// Pixel storage modes. Value 0x9240.
        /// </summary>
        public const uint UNPACK_FLIP_Y_WEBGL = 0x9240;
        /// <summary>
        /// Pixel storage modes. Value 0x9241.
        /// </summary>
        public const uint UNPACK_PREMULTIPLY_ALPHA_WEBGL = 0x9241;
        /// <summary>
        /// Pixel storage modes. Value 0x9243.
        /// </summary>
        public const uint UNPACK_COLORSPACE_CONVERSION_WEBGL = 0x9243;
        #endregion

        // =====================================================================
        // WebGL 2 constants
        // =====================================================================

        #region WebGL2 Getting GL parameter information
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x0C02.
        /// </summary>
        public const uint READ_BUFFER = 0x0C02;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x0CF2.
        /// </summary>
        public const uint UNPACK_ROW_LENGTH = 0x0CF2;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x0CF3.
        /// </summary>
        public const uint UNPACK_SKIP_ROWS = 0x0CF3;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x0CF4.
        /// </summary>
        public const uint UNPACK_SKIP_PIXELS = 0x0CF4;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x0D02.
        /// </summary>
        public const uint PACK_ROW_LENGTH = 0x0D02;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x0D03.
        /// </summary>
        public const uint PACK_SKIP_ROWS = 0x0D03;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x0D04.
        /// </summary>
        public const uint PACK_SKIP_PIXELS = 0x0D04;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x806A.
        /// </summary>
        public const uint TEXTURE_BINDING_3D = 0x806A;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x806D.
        /// </summary>
        public const uint UNPACK_SKIP_IMAGES = 0x806D;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x806E.
        /// </summary>
        public const uint UNPACK_IMAGE_HEIGHT = 0x806E;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x8073.
        /// </summary>
        public const uint MAX_3D_TEXTURE_SIZE = 0x8073;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x80E8.
        /// </summary>
        public const uint MAX_ELEMENTS_VERTICES = 0x80E8;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x80E9.
        /// </summary>
        public const uint MAX_ELEMENTS_INDICES = 0x80E9;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x84FD.
        /// </summary>
        public const uint MAX_TEXTURE_LOD_BIAS = 0x84FD;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x8B49.
        /// </summary>
        public const uint MAX_FRAGMENT_UNIFORM_COMPONENTS = 0x8B49;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x8B4A.
        /// </summary>
        public const uint MAX_VERTEX_UNIFORM_COMPONENTS = 0x8B4A;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x88FF.
        /// </summary>
        public const uint MAX_ARRAY_TEXTURE_LAYERS = 0x88FF;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x8904.
        /// </summary>
        public const uint MIN_PROGRAM_TEXEL_OFFSET = 0x8904;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x8905.
        /// </summary>
        public const uint MAX_PROGRAM_TEXEL_OFFSET = 0x8905;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x8B4B.
        /// </summary>
        public const uint MAX_VARYING_COMPONENTS = 0x8B4B;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x8B8B.
        /// </summary>
        public const uint FRAGMENT_SHADER_DERIVATIVE_HINT = 0x8B8B;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x8C89.
        /// </summary>
        public const uint RASTERIZER_DISCARD = 0x8C89;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x85B5.
        /// </summary>
        public const uint VERTEX_ARRAY_BINDING = 0x85B5;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x9122.
        /// </summary>
        public const uint MAX_VERTEX_OUTPUT_COMPONENTS = 0x9122;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x9125.
        /// </summary>
        public const uint MAX_FRAGMENT_INPUT_COMPONENTS = 0x9125;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x9111.
        /// </summary>
        public const uint MAX_SERVER_WAIT_TIMEOUT = 0x9111;
        /// <summary>
        /// WebGL2 Getting GL parameter information. Value 0x8D6B.
        /// </summary>
        public const uint MAX_ELEMENT_INDEX = 0x8D6B;
        #endregion

        #region WebGL2 Textures
        /// <summary>
        /// WebGL2 Textures. Value 0x1903.
        /// </summary>
        public const uint RED = 0x1903;
        /// <summary>
        /// WebGL2 Textures. Value 0x8051.
        /// </summary>
        public const uint RGB8 = 0x8051;
        /// <summary>
        /// WebGL2 Textures. Value 0x8058.
        /// </summary>
        public const uint RGBA8 = 0x8058;
        /// <summary>
        /// WebGL2 Textures. Value 0x8059.
        /// </summary>
        public const uint RGB10_A2 = 0x8059;
        /// <summary>
        /// WebGL2 Textures. Value 0x806F.
        /// </summary>
        public const uint TEXTURE_3D = 0x806F;
        /// <summary>
        /// WebGL2 Textures. Value 0x8072.
        /// </summary>
        public const uint TEXTURE_WRAP_R = 0x8072;
        /// <summary>
        /// WebGL2 Textures. Value 0x813A.
        /// </summary>
        public const uint TEXTURE_MIN_LOD = 0x813A;
        /// <summary>
        /// WebGL2 Textures. Value 0x813B.
        /// </summary>
        public const uint TEXTURE_MAX_LOD = 0x813B;
        /// <summary>
        /// WebGL2 Textures. Value 0x813C.
        /// </summary>
        public const uint TEXTURE_BASE_LEVEL = 0x813C;
        /// <summary>
        /// WebGL2 Textures. Value 0x813D.
        /// </summary>
        public const uint TEXTURE_MAX_LEVEL = 0x813D;
        /// <summary>
        /// WebGL2 Textures. Value 0x884C.
        /// </summary>
        public const uint TEXTURE_COMPARE_MODE = 0x884C;
        /// <summary>
        /// WebGL2 Textures. Value 0x884D.
        /// </summary>
        public const uint TEXTURE_COMPARE_FUNC = 0x884D;
        /// <summary>
        /// WebGL2 Textures. Value 0x8C40.
        /// </summary>
        public const uint SRGB = 0x8C40;
        /// <summary>
        /// WebGL2 Textures. Value 0x8C41.
        /// </summary>
        public const uint SRGB8 = 0x8C41;
        /// <summary>
        /// WebGL2 Textures. Value 0x8C43.
        /// </summary>
        public const uint SRGB8_ALPHA8 = 0x8C43;
        /// <summary>
        /// WebGL2 Textures. Value 0x884E.
        /// </summary>
        public const uint COMPARE_REF_TO_TEXTURE = 0x884E;
        /// <summary>
        /// WebGL2 Textures. Value 0x8814.
        /// </summary>
        public const uint RGBA32F = 0x8814;
        /// <summary>
        /// WebGL2 Textures. Value 0x8815.
        /// </summary>
        public const uint RGB32F = 0x8815;
        /// <summary>
        /// WebGL2 Textures. Value 0x881A.
        /// </summary>
        public const uint RGBA16F = 0x881A;
        /// <summary>
        /// WebGL2 Textures. Value 0x881B.
        /// </summary>
        public const uint RGB16F = 0x881B;
        /// <summary>
        /// WebGL2 Textures. Value 0x8C1A.
        /// </summary>
        public const uint TEXTURE_2D_ARRAY = 0x8C1A;
        /// <summary>
        /// WebGL2 Textures. Value 0x8C1D.
        /// </summary>
        public const uint TEXTURE_BINDING_2D_ARRAY = 0x8C1D;
        /// <summary>
        /// WebGL2 Textures. Value 0x8C3A.
        /// </summary>
        public const uint R11F_G11F_B10F = 0x8C3A;
        /// <summary>
        /// WebGL2 Textures. Value 0x8C3D.
        /// </summary>
        public const uint RGB9_E5 = 0x8C3D;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D70.
        /// </summary>
        public const uint RGBA32UI = 0x8D70;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D71.
        /// </summary>
        public const uint RGB32UI = 0x8D71;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D76.
        /// </summary>
        public const uint RGBA16UI = 0x8D76;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D77.
        /// </summary>
        public const uint RGB16UI = 0x8D77;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D7C.
        /// </summary>
        public const uint RGBA8UI = 0x8D7C;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D7D.
        /// </summary>
        public const uint RGB8UI = 0x8D7D;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D82.
        /// </summary>
        public const uint RGBA32I = 0x8D82;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D83.
        /// </summary>
        public const uint RGB32I = 0x8D83;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D88.
        /// </summary>
        public const uint RGBA16I = 0x8D88;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D89.
        /// </summary>
        public const uint RGB16I = 0x8D89;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D8E.
        /// </summary>
        public const uint RGBA8I = 0x8D8E;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D8F.
        /// </summary>
        public const uint RGB8I = 0x8D8F;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D94.
        /// </summary>
        public const uint RED_INTEGER = 0x8D94;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D98.
        /// </summary>
        public const uint RGB_INTEGER = 0x8D98;
        /// <summary>
        /// WebGL2 Textures. Value 0x8D99.
        /// </summary>
        public const uint RGBA_INTEGER = 0x8D99;
        /// <summary>
        /// WebGL2 Textures. Value 0x8229.
        /// </summary>
        public const uint R8 = 0x8229;
        /// <summary>
        /// WebGL2 Textures. Value 0x822B.
        /// </summary>
        public const uint RG8 = 0x822B;
        /// <summary>
        /// WebGL2 Textures. Value 0x822D.
        /// </summary>
        public const uint R16F = 0x822D;
        /// <summary>
        /// WebGL2 Textures. Value 0x822E.
        /// </summary>
        public const uint R32F = 0x822E;
        /// <summary>
        /// WebGL2 Textures. Value 0x822F.
        /// </summary>
        public const uint RG16F = 0x822F;
        /// <summary>
        /// WebGL2 Textures. Value 0x8230.
        /// </summary>
        public const uint RG32F = 0x8230;
        /// <summary>
        /// WebGL2 Textures. Value 0x8231.
        /// </summary>
        public const uint R8I = 0x8231;
        /// <summary>
        /// WebGL2 Textures. Value 0x8232.
        /// </summary>
        public const uint R8UI = 0x8232;
        /// <summary>
        /// WebGL2 Textures. Value 0x8233.
        /// </summary>
        public const uint R16I = 0x8233;
        /// <summary>
        /// WebGL2 Textures. Value 0x8234.
        /// </summary>
        public const uint R16UI = 0x8234;
        /// <summary>
        /// WebGL2 Textures. Value 0x8235.
        /// </summary>
        public const uint R32I = 0x8235;
        /// <summary>
        /// WebGL2 Textures. Value 0x8236.
        /// </summary>
        public const uint R32UI = 0x8236;
        /// <summary>
        /// WebGL2 Textures. Value 0x8237.
        /// </summary>
        public const uint RG8I = 0x8237;
        /// <summary>
        /// WebGL2 Textures. Value 0x8238.
        /// </summary>
        public const uint RG8UI = 0x8238;
        /// <summary>
        /// WebGL2 Textures. Value 0x8239.
        /// </summary>
        public const uint RG16I = 0x8239;
        /// <summary>
        /// WebGL2 Textures. Value 0x823A.
        /// </summary>
        public const uint RG16UI = 0x823A;
        /// <summary>
        /// WebGL2 Textures. Value 0x823B.
        /// </summary>
        public const uint RG32I = 0x823B;
        /// <summary>
        /// WebGL2 Textures. Value 0x823C.
        /// </summary>
        public const uint RG32UI = 0x823C;
        /// <summary>
        /// WebGL2 Textures. Value 0x906F.
        /// </summary>
        public const uint RGB10_A2UI = 0x906F;
        /// <summary>
        /// WebGL2 Textures. Value 0x912F.
        /// </summary>
        public const uint TEXTURE_IMMUTABLE_FORMAT = 0x912F;
        /// <summary>
        /// WebGL2 Textures. Value 0x82DF.
        /// </summary>
        public const uint TEXTURE_IMMUTABLE_LEVELS = 0x82DF;
        #endregion

        #region WebGL2 Pixel types
        /// <summary>
        /// WebGL2 Pixel types. Value 0x8368.
        /// </summary>
        public const uint UNSIGNED_INT_2_10_10_10_REV = 0x8368;
        /// <summary>
        /// WebGL2 Pixel types. Value 0x8C3B.
        /// </summary>
        public const uint UNSIGNED_INT_10F_11F_11F_REV = 0x8C3B;
        /// <summary>
        /// WebGL2 Pixel types. Value 0x8C3E.
        /// </summary>
        public const uint UNSIGNED_INT_5_9_9_9_REV = 0x8C3E;
        /// <summary>
        /// WebGL2 Pixel types. Value 0x8DAD.
        /// </summary>
        public const uint FLOAT_32_UNSIGNED_INT_24_8_REV = 0x8DAD;
        /// <summary>
        /// WebGL2 Pixel types. Value 0x140B.
        /// </summary>
        public const uint HALF_FLOAT = 0x140B;
        /// <summary>
        /// WebGL2 Pixel types. Value 0x8227.
        /// </summary>
        public const uint RG = 0x8227;
        /// <summary>
        /// WebGL2 Pixel types. Value 0x8228.
        /// </summary>
        public const uint RG_INTEGER = 0x8228;
        /// <summary>
        /// WebGL2 Pixel types. Value 0x8D9F.
        /// </summary>
        public const uint INT_2_10_10_10_REV = 0x8D9F;
        #endregion

        #region WebGL2 Queries
        /// <summary>
        /// WebGL2 Queries. Value 0x8865.
        /// </summary>
        public const uint CURRENT_QUERY = 0x8865;
        /// <summary>
        /// WebGL2 Queries. Value 0x8866.
        /// </summary>
        public const uint QUERY_RESULT = 0x8866;
        /// <summary>
        /// WebGL2 Queries. Value 0x8867.
        /// </summary>
        public const uint QUERY_RESULT_AVAILABLE = 0x8867;
        /// <summary>
        /// WebGL2 Queries. Value 0x8C2F.
        /// </summary>
        public const uint ANY_SAMPLES_PASSED = 0x8C2F;
        /// <summary>
        /// WebGL2 Queries. Value 0x8D6A.
        /// </summary>
        public const uint ANY_SAMPLES_PASSED_CONSERVATIVE = 0x8D6A;
        #endregion

        #region WebGL2 Draw buffers
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8824.
        /// </summary>
        public const uint MAX_DRAW_BUFFERS = 0x8824;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8825.
        /// </summary>
        public const uint DRAW_BUFFER0 = 0x8825;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8826.
        /// </summary>
        public const uint DRAW_BUFFER1 = 0x8826;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8827.
        /// </summary>
        public const uint DRAW_BUFFER2 = 0x8827;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8828.
        /// </summary>
        public const uint DRAW_BUFFER3 = 0x8828;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8829.
        /// </summary>
        public const uint DRAW_BUFFER4 = 0x8829;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x882A.
        /// </summary>
        public const uint DRAW_BUFFER5 = 0x882A;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x882B.
        /// </summary>
        public const uint DRAW_BUFFER6 = 0x882B;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x882C.
        /// </summary>
        public const uint DRAW_BUFFER7 = 0x882C;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x882D.
        /// </summary>
        public const uint DRAW_BUFFER8 = 0x882D;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x882E.
        /// </summary>
        public const uint DRAW_BUFFER9 = 0x882E;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x882F.
        /// </summary>
        public const uint DRAW_BUFFER10 = 0x882F;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8830.
        /// </summary>
        public const uint DRAW_BUFFER11 = 0x8830;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8831.
        /// </summary>
        public const uint DRAW_BUFFER12 = 0x8831;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8832.
        /// </summary>
        public const uint DRAW_BUFFER13 = 0x8832;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8833.
        /// </summary>
        public const uint DRAW_BUFFER14 = 0x8833;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8834.
        /// </summary>
        public const uint DRAW_BUFFER15 = 0x8834;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CDF.
        /// </summary>
        public const uint MAX_COLOR_ATTACHMENTS = 0x8CDF;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CE1.
        /// </summary>
        public const uint COLOR_ATTACHMENT1 = 0x8CE1;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CE2.
        /// </summary>
        public const uint COLOR_ATTACHMENT2 = 0x8CE2;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CE3.
        /// </summary>
        public const uint COLOR_ATTACHMENT3 = 0x8CE3;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CE4.
        /// </summary>
        public const uint COLOR_ATTACHMENT4 = 0x8CE4;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CE5.
        /// </summary>
        public const uint COLOR_ATTACHMENT5 = 0x8CE5;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CE6.
        /// </summary>
        public const uint COLOR_ATTACHMENT6 = 0x8CE6;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CE7.
        /// </summary>
        public const uint COLOR_ATTACHMENT7 = 0x8CE7;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CE8.
        /// </summary>
        public const uint COLOR_ATTACHMENT8 = 0x8CE8;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CE9.
        /// </summary>
        public const uint COLOR_ATTACHMENT9 = 0x8CE9;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CEA.
        /// </summary>
        public const uint COLOR_ATTACHMENT10 = 0x8CEA;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CEB.
        /// </summary>
        public const uint COLOR_ATTACHMENT11 = 0x8CEB;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CEC.
        /// </summary>
        public const uint COLOR_ATTACHMENT12 = 0x8CEC;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CED.
        /// </summary>
        public const uint COLOR_ATTACHMENT13 = 0x8CED;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CEE.
        /// </summary>
        public const uint COLOR_ATTACHMENT14 = 0x8CEE;
        /// <summary>
        /// WebGL2 Draw buffers. Value 0x8CEF.
        /// </summary>
        public const uint COLOR_ATTACHMENT15 = 0x8CEF;
        #endregion

        #region WebGL2 Samplers
        /// <summary>
        /// WebGL2 Samplers. Value 0x8B5F.
        /// </summary>
        public const uint SAMPLER_3D = 0x8B5F;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8B62.
        /// </summary>
        public const uint SAMPLER_2D_SHADOW = 0x8B62;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DC1.
        /// </summary>
        public const uint SAMPLER_2D_ARRAY = 0x8DC1;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DC4.
        /// </summary>
        public const uint SAMPLER_2D_ARRAY_SHADOW = 0x8DC4;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DC5.
        /// </summary>
        public const uint SAMPLER_CUBE_SHADOW = 0x8DC5;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DCA.
        /// </summary>
        public const uint INT_SAMPLER_2D = 0x8DCA;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DCB.
        /// </summary>
        public const uint INT_SAMPLER_3D = 0x8DCB;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DCC.
        /// </summary>
        public const uint INT_SAMPLER_CUBE = 0x8DCC;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DCF.
        /// </summary>
        public const uint INT_SAMPLER_2D_ARRAY = 0x8DCF;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DD2.
        /// </summary>
        public const uint UNSIGNED_INT_SAMPLER_2D = 0x8DD2;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DD3.
        /// </summary>
        public const uint UNSIGNED_INT_SAMPLER_3D = 0x8DD3;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DD4.
        /// </summary>
        public const uint UNSIGNED_INT_SAMPLER_CUBE = 0x8DD4;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8DD7.
        /// </summary>
        public const uint UNSIGNED_INT_SAMPLER_2D_ARRAY = 0x8DD7;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8D57.
        /// </summary>
        public const uint MAX_SAMPLES = 0x8D57;
        /// <summary>
        /// WebGL2 Samplers. Value 0x8919.
        /// </summary>
        public const uint SAMPLER_BINDING = 0x8919;
        #endregion

        #region WebGL2 Buffers
        /// <summary>
        /// WebGL2 Buffers. Value 0x88EB.
        /// </summary>
        public const uint PIXEL_PACK_BUFFER = 0x88EB;
        /// <summary>
        /// WebGL2 Buffers. Value 0x88EC.
        /// </summary>
        public const uint PIXEL_UNPACK_BUFFER = 0x88EC;
        /// <summary>
        /// WebGL2 Buffers. Value 0x88ED.
        /// </summary>
        public const uint PIXEL_PACK_BUFFER_BINDING = 0x88ED;
        /// <summary>
        /// WebGL2 Buffers. Value 0x88EF.
        /// </summary>
        public const uint PIXEL_UNPACK_BUFFER_BINDING = 0x88EF;
        /// <summary>
        /// WebGL2 Buffers. Value 0x8F36.
        /// </summary>
        public const uint COPY_READ_BUFFER = 0x8F36;
        /// <summary>
        /// WebGL2 Buffers. Value 0x8F37.
        /// </summary>
        public const uint COPY_WRITE_BUFFER = 0x8F37;
        /// <summary>
        /// WebGL2 Buffers. Value 0x8F36.
        /// </summary>
        public const uint COPY_READ_BUFFER_BINDING = 0x8F36;
        /// <summary>
        /// WebGL2 Buffers. Value 0x8F37.
        /// </summary>
        public const uint COPY_WRITE_BUFFER_BINDING = 0x8F37;
        #endregion

        #region WebGL2 Data types
        /// <summary>
        /// WebGL2 Data types. Value 0x8B65.
        /// </summary>
        public const uint FLOAT_MAT2x3 = 0x8B65;
        /// <summary>
        /// WebGL2 Data types. Value 0x8B66.
        /// </summary>
        public const uint FLOAT_MAT2x4 = 0x8B66;
        /// <summary>
        /// WebGL2 Data types. Value 0x8B67.
        /// </summary>
        public const uint FLOAT_MAT3x2 = 0x8B67;
        /// <summary>
        /// WebGL2 Data types. Value 0x8B68.
        /// </summary>
        public const uint FLOAT_MAT3x4 = 0x8B68;
        /// <summary>
        /// WebGL2 Data types. Value 0x8B69.
        /// </summary>
        public const uint FLOAT_MAT4x2 = 0x8B69;
        /// <summary>
        /// WebGL2 Data types. Value 0x8B6A.
        /// </summary>
        public const uint FLOAT_MAT4x3 = 0x8B6A;
        /// <summary>
        /// WebGL2 Data types. Value 0x8DC6.
        /// </summary>
        public const uint UNSIGNED_INT_VEC2 = 0x8DC6;
        /// <summary>
        /// WebGL2 Data types. Value 0x8DC7.
        /// </summary>
        public const uint UNSIGNED_INT_VEC3 = 0x8DC7;
        /// <summary>
        /// WebGL2 Data types. Value 0x8DC8.
        /// </summary>
        public const uint UNSIGNED_INT_VEC4 = 0x8DC8;
        /// <summary>
        /// WebGL2 Data types. Value 0x8C17.
        /// </summary>
        public const uint UNSIGNED_NORMALIZED = 0x8C17;
        /// <summary>
        /// WebGL2 Data types. Value 0x8F9C.
        /// </summary>
        public const uint SIGNED_NORMALIZED = 0x8F9C;
        #endregion

        #region WebGL2 Vertex attributes
        /// <summary>
        /// WebGL2 Vertex attributes. Value 0x88FD.
        /// </summary>
        public const uint VERTEX_ATTRIB_ARRAY_INTEGER = 0x88FD;
        /// <summary>
        /// WebGL2 Vertex attributes. Value 0x88FE.
        /// </summary>
        public const uint VERTEX_ATTRIB_ARRAY_DIVISOR = 0x88FE;
        #endregion

        #region WebGL2 Transform feedback
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C7F.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK_BUFFER_MODE = 0x8C7F;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C80.
        /// </summary>
        public const uint MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS = 0x8C80;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C83.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK_VARYINGS = 0x8C83;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C84.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK_BUFFER_START = 0x8C84;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C85.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK_BUFFER_SIZE = 0x8C85;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C88.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN = 0x8C88;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C8A.
        /// </summary>
        public const uint MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = 0x8C8A;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C8B.
        /// </summary>
        public const uint MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS = 0x8C8B;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C8C.
        /// </summary>
        public const uint INTERLEAVED_ATTRIBS = 0x8C8C;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C8D.
        /// </summary>
        public const uint SEPARATE_ATTRIBS = 0x8C8D;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C8E.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK_BUFFER = 0x8C8E;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8C8F.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK_BUFFER_BINDING = 0x8C8F;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8E22.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK = 0x8E22;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8E23.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK_PAUSED = 0x8E23;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8E24.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK_ACTIVE = 0x8E24;
        /// <summary>
        /// WebGL2 Transform feedback. Value 0x8E25.
        /// </summary>
        public const uint TRANSFORM_FEEDBACK_BINDING = 0x8E25;
        #endregion

        #region WebGL2 Framebuffers and renderbuffers
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8210.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = 0x8210;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8211.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = 0x8211;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8212.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_RED_SIZE = 0x8212;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8213.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_GREEN_SIZE = 0x8213;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8214.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_BLUE_SIZE = 0x8214;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8215.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE = 0x8215;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8216.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE = 0x8216;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8217.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE = 0x8217;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8218.
        /// </summary>
        public const uint FRAMEBUFFER_DEFAULT = 0x8218;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x88F0.
        /// </summary>
        public const uint DEPTH24_STENCIL8 = 0x88F0;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8CA6.
        /// </summary>
        public const uint DRAW_FRAMEBUFFER_BINDING = 0x8CA6;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8CA8.
        /// </summary>
        public const uint READ_FRAMEBUFFER = 0x8CA8;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8CA9.
        /// </summary>
        public const uint DRAW_FRAMEBUFFER = 0x8CA9;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8CAA.
        /// </summary>
        public const uint READ_FRAMEBUFFER_BINDING = 0x8CAA;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8CAB.
        /// </summary>
        public const uint RENDERBUFFER_SAMPLES = 0x8CAB;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8CD4.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER = 0x8CD4;
        /// <summary>
        /// WebGL2 Framebuffers and renderbuffers. Value 0x8D56.
        /// </summary>
        public const uint FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x8D56;
        #endregion

        #region WebGL2 Uniforms
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A11.
        /// </summary>
        public const uint UNIFORM_BUFFER = 0x8A11;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A28.
        /// </summary>
        public const uint UNIFORM_BUFFER_BINDING = 0x8A28;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A29.
        /// </summary>
        public const uint UNIFORM_BUFFER_START = 0x8A29;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A2A.
        /// </summary>
        public const uint UNIFORM_BUFFER_SIZE = 0x8A2A;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A2B.
        /// </summary>
        public const uint MAX_VERTEX_UNIFORM_BLOCKS = 0x8A2B;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A2C.
        /// </summary>
        public const uint MAX_FRAGMENT_UNIFORM_BLOCKS = 0x8A2C;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A2E.
        /// </summary>
        public const uint MAX_COMBINED_UNIFORM_BLOCKS = 0x8A2E;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A2F.
        /// </summary>
        public const uint MAX_UNIFORM_BUFFER_BINDINGS = 0x8A2F;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A30.
        /// </summary>
        public const uint MAX_UNIFORM_BLOCK_SIZE = 0x8A30;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A31.
        /// </summary>
        public const uint MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS = 0x8A31;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A33.
        /// </summary>
        public const uint MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS = 0x8A33;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A34.
        /// </summary>
        public const uint UNIFORM_BUFFER_OFFSET_ALIGNMENT = 0x8A34;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A36.
        /// </summary>
        public const uint ACTIVE_UNIFORM_BLOCKS = 0x8A36;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A37.
        /// </summary>
        public const uint UNIFORM_TYPE = 0x8A37;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A38.
        /// </summary>
        public const uint UNIFORM_SIZE = 0x8A38;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A3A.
        /// </summary>
        public const uint UNIFORM_BLOCK_INDEX = 0x8A3A;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A3B.
        /// </summary>
        public const uint UNIFORM_OFFSET = 0x8A3B;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A3C.
        /// </summary>
        public const uint UNIFORM_ARRAY_STRIDE = 0x8A3C;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A3D.
        /// </summary>
        public const uint UNIFORM_MATRIX_STRIDE = 0x8A3D;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A3E.
        /// </summary>
        public const uint UNIFORM_IS_ROW_MAJOR = 0x8A3E;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A3F.
        /// </summary>
        public const uint UNIFORM_BLOCK_BINDING = 0x8A3F;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A40.
        /// </summary>
        public const uint UNIFORM_BLOCK_DATA_SIZE = 0x8A40;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A42.
        /// </summary>
        public const uint UNIFORM_BLOCK_ACTIVE_UNIFORMS = 0x8A42;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A43.
        /// </summary>
        public const uint UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES = 0x8A43;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A44.
        /// </summary>
        public const uint UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER = 0x8A44;
        /// <summary>
        /// WebGL2 Uniforms. Value 0x8A46.
        /// </summary>
        public const uint UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER = 0x8A46;
        #endregion

        #region WebGL2 Sync objects
        /// <summary>
        /// WebGL2 Sync objects. Value 0x9112.
        /// </summary>
        public const uint OBJECT_TYPE = 0x9112;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x9113.
        /// </summary>
        public const uint SYNC_CONDITION = 0x9113;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x9114.
        /// </summary>
        public const uint SYNC_STATUS = 0x9114;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x9115.
        /// </summary>
        public const uint SYNC_FLAGS = 0x9115;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x9116.
        /// </summary>
        public const uint SYNC_FENCE = 0x9116;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x9117.
        /// </summary>
        public const uint SYNC_GPU_COMMANDS_COMPLETE = 0x9117;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x9118.
        /// </summary>
        public const uint UNSIGNALED = 0x9118;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x9119.
        /// </summary>
        public const uint SIGNALED = 0x9119;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x911A.
        /// </summary>
        public const uint ALREADY_SIGNALED = 0x911A;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x911B.
        /// </summary>
        public const uint TIMEOUT_EXPIRED = 0x911B;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x911C.
        /// </summary>
        public const uint CONDITION_SATISFIED = 0x911C;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x911D.
        /// </summary>
        public const uint WAIT_FAILED = 0x911D;
        /// <summary>
        /// WebGL2 Sync objects. Value 0x00000001.
        /// </summary>
        public const uint SYNC_FLUSH_COMMANDS_BIT = 0x00000001;
        #endregion

        #region WebGL2 Miscellaneous
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x1800.
        /// </summary>
        public const uint COLOR = 0x1800;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x1801.
        /// </summary>
        public const uint DEPTH = 0x1801;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x1802.
        /// </summary>
        public const uint STENCIL = 0x1802;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x8007.
        /// </summary>
        public const uint MIN = 0x8007;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x8008.
        /// </summary>
        public const uint MAX = 0x8008;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x81A6.
        /// </summary>
        public const uint DEPTH_COMPONENT24 = 0x81A6;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x88E1.
        /// </summary>
        public const uint STREAM_READ = 0x88E1;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x88E2.
        /// </summary>
        public const uint STREAM_COPY = 0x88E2;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x88E5.
        /// </summary>
        public const uint STATIC_READ = 0x88E5;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x88E6.
        /// </summary>
        public const uint STATIC_COPY = 0x88E6;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x88E9.
        /// </summary>
        public const uint DYNAMIC_READ = 0x88E9;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x88EA.
        /// </summary>
        public const uint DYNAMIC_COPY = 0x88EA;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x8CAC.
        /// </summary>
        public const uint DEPTH_COMPONENT32F = 0x8CAC;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x8CAD.
        /// </summary>
        public const uint DEPTH32F_STENCIL8 = 0x8CAD;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0xFFFFFFFF.
        /// </summary>
        public const uint INVALID_INDEX = 0xFFFFFFFF;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0xFFFFFFFF.
        /// </summary>
        public const uint TIMEOUT_IGNORED = 0xFFFFFFFF;
        /// <summary>
        /// WebGL2 Miscellaneous. Value 0x9247.
        /// </summary>
        public const uint MAX_CLIENT_WAIT_TIMEOUT_WEBGL = 0x9247;
        #endregion

        // =====================================================================
        // Extension constants
        // =====================================================================

        #region ANGLE_instanced_arrays
        /// <summary>
        /// ANGLE_instanced_arrays. Value 0x88FE.
        /// </summary>
        public const uint VERTEX_ATTRIB_ARRAY_DIVISOR_ANGLE = 0x88FE;
        #endregion

        #region WEBGL_debug_renderer_info
        /// <summary>
        /// WEBGL_debug_renderer_info. Value 0x9245.
        /// </summary>
        public const uint UNMASKED_VENDOR_WEBGL = 0x9245;
        /// <summary>
        /// WEBGL_debug_renderer_info. Value 0x9246.
        /// </summary>
        public const uint UNMASKED_RENDERER_WEBGL = 0x9246;
        #endregion

        #region EXT_texture_filter_anisotropic
        /// <summary>
        /// EXT_texture_filter_anisotropic. Value 0x84FF.
        /// </summary>
        public const uint MAX_TEXTURE_MAX_ANISOTROPY_EXT = 0x84FF;
        /// <summary>
        /// EXT_texture_filter_anisotropic. Value 0x84FE.
        /// </summary>
        public const uint TEXTURE_MAX_ANISOTROPY_EXT = 0x84FE;
        #endregion

        #region WEBGL_compressed_texture_s3tc
        /// <summary>
        /// WEBGL_compressed_texture_s3tc. Value 0x83F0.
        /// </summary>
        public const uint COMPRESSED_RGB_S3TC_DXT1_EXT = 0x83F0;
        /// <summary>
        /// WEBGL_compressed_texture_s3tc. Value 0x83F1.
        /// </summary>
        public const uint COMPRESSED_RGBA_S3TC_DXT1_EXT = 0x83F1;
        /// <summary>
        /// WEBGL_compressed_texture_s3tc. Value 0x83F2.
        /// </summary>
        public const uint COMPRESSED_RGBA_S3TC_DXT3_EXT = 0x83F2;
        /// <summary>
        /// WEBGL_compressed_texture_s3tc. Value 0x83F3.
        /// </summary>
        public const uint COMPRESSED_RGBA_S3TC_DXT5_EXT = 0x83F3;
        #endregion

        #region WEBGL_compressed_texture_etc
        /// <summary>
        /// WEBGL_compressed_texture_etc. Value 0x9270.
        /// </summary>
        public const uint COMPRESSED_R11_EAC = 0x9270;
        /// <summary>
        /// WEBGL_compressed_texture_etc. Value 0x9271.
        /// </summary>
        public const uint COMPRESSED_SIGNED_R11_EAC = 0x9271;
        /// <summary>
        /// WEBGL_compressed_texture_etc. Value 0x9272.
        /// </summary>
        public const uint COMPRESSED_RG11_EAC = 0x9272;
        /// <summary>
        /// WEBGL_compressed_texture_etc. Value 0x9273.
        /// </summary>
        public const uint COMPRESSED_SIGNED_RG11_EAC = 0x9273;
        /// <summary>
        /// WEBGL_compressed_texture_etc. Value 0x9274.
        /// </summary>
        public const uint COMPRESSED_RGB8_ETC2 = 0x9274;
        /// <summary>
        /// WEBGL_compressed_texture_etc. Value 0x9275.
        /// </summary>
        public const uint COMPRESSED_RGBA8_ETC2_EAC = 0x9275;
        /// <summary>
        /// WEBGL_compressed_texture_etc. Value 0x9276.
        /// </summary>
        public const uint COMPRESSED_SRGB8_ETC2 = 0x9276;
        /// <summary>
        /// WEBGL_compressed_texture_etc. Value 0x9277.
        /// </summary>
        public const uint COMPRESSED_SRGB8_ALPHA8_ETC2_EAC = 0x9277;
        /// <summary>
        /// WEBGL_compressed_texture_etc. Value 0x9278.
        /// </summary>
        public const uint COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9278;
        /// <summary>
        /// WEBGL_compressed_texture_etc. Value 0x9279.
        /// </summary>
        public const uint COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9279;
        #endregion

        #region WEBGL_compressed_texture_pvrtc
        /// <summary>
        /// WEBGL_compressed_texture_pvrtc. Value 0x8C00.
        /// </summary>
        public const uint COMPRESSED_RGB_PVRTC_4BPPV1_IMG = 0x8C00;
        /// <summary>
        /// WEBGL_compressed_texture_pvrtc. Value 0x8C02.
        /// </summary>
        public const uint COMPRESSED_RGBA_PVRTC_4BPPV1_IMG = 0x8C02;
        /// <summary>
        /// WEBGL_compressed_texture_pvrtc. Value 0x8C01.
        /// </summary>
        public const uint COMPRESSED_RGB_PVRTC_2BPPV1_IMG = 0x8C01;
        /// <summary>
        /// WEBGL_compressed_texture_pvrtc. Value 0x8C03.
        /// </summary>
        public const uint COMPRESSED_RGBA_PVRTC_2BPPV1_IMG = 0x8C03;
        #endregion

        #region WEBGL_compressed_texture_etc1
        /// <summary>
        /// WEBGL_compressed_texture_etc1. Value 0x8D64.
        /// </summary>
        public const uint COMPRESSED_RGB_ETC1_WEBGL = 0x8D64;
        #endregion

        #region WEBGL_depth_texture
        /// <summary>
        /// WEBGL_depth_texture. Value 0x84FA.
        /// </summary>
        public const uint UNSIGNED_INT_24_8_WEBGL = 0x84FA;
        #endregion

        #region OES_texture_half_float
        /// <summary>
        /// OES_texture_half_float. Value 0x8D61.
        /// </summary>
        public const uint HALF_FLOAT_OES = 0x8D61;
        #endregion

        #region WEBGL_color_buffer_float
        /// <summary>
        /// WEBGL_color_buffer_float. Value 0x8814.
        /// </summary>
        public const uint RGBA32F_EXT = 0x8814;
        /// <summary>
        /// WEBGL_color_buffer_float. Value 0x8211.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE_EXT = 0x8211;
        /// <summary>
        /// WEBGL_color_buffer_float. Value 0x8C17.
        /// </summary>
        public const uint UNSIGNED_NORMALIZED_EXT = 0x8C17;
        #endregion

        #region EXT_blend_minmax
        /// <summary>
        /// EXT_blend_minmax. Value 0x8007.
        /// </summary>
        public const uint MIN_EXT = 0x8007;
        /// <summary>
        /// EXT_blend_minmax. Value 0x8008.
        /// </summary>
        public const uint MAX_EXT = 0x8008;
        #endregion

        #region EXT_sRGB
        /// <summary>
        /// EXT_sRGB. Value 0x8C40.
        /// </summary>
        public const uint SRGB_EXT = 0x8C40;
        /// <summary>
        /// EXT_sRGB. Value 0x8C42.
        /// </summary>
        public const uint SRGB_ALPHA_EXT = 0x8C42;
        /// <summary>
        /// EXT_sRGB. Value 0x8C43.
        /// </summary>
        public const uint SRGB8_ALPHA8_EXT = 0x8C43;
        /// <summary>
        /// EXT_sRGB. Value 0x8210.
        /// </summary>
        public const uint FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING_EXT = 0x8210;
        #endregion

        #region OES_standard_derivatives
        /// <summary>
        /// OES_standard_derivatives. Value 0x8B8B.
        /// </summary>
        public const uint FRAGMENT_SHADER_DERIVATIVE_HINT_OES = 0x8B8B;
        #endregion

        #region WEBGL_draw_buffers
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CE0.
        /// </summary>
        public const uint COLOR_ATTACHMENT0_WEBGL = 0x8CE0;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CE1.
        /// </summary>
        public const uint COLOR_ATTACHMENT1_WEBGL = 0x8CE1;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CE2.
        /// </summary>
        public const uint COLOR_ATTACHMENT2_WEBGL = 0x8CE2;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CE3.
        /// </summary>
        public const uint COLOR_ATTACHMENT3_WEBGL = 0x8CE3;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CE4.
        /// </summary>
        public const uint COLOR_ATTACHMENT4_WEBGL = 0x8CE4;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CE5.
        /// </summary>
        public const uint COLOR_ATTACHMENT5_WEBGL = 0x8CE5;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CE6.
        /// </summary>
        public const uint COLOR_ATTACHMENT6_WEBGL = 0x8CE6;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CE7.
        /// </summary>
        public const uint COLOR_ATTACHMENT7_WEBGL = 0x8CE7;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CE8.
        /// </summary>
        public const uint COLOR_ATTACHMENT8_WEBGL = 0x8CE8;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CE9.
        /// </summary>
        public const uint COLOR_ATTACHMENT9_WEBGL = 0x8CE9;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CEA.
        /// </summary>
        public const uint COLOR_ATTACHMENT10_WEBGL = 0x8CEA;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CEB.
        /// </summary>
        public const uint COLOR_ATTACHMENT11_WEBGL = 0x8CEB;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CEC.
        /// </summary>
        public const uint COLOR_ATTACHMENT12_WEBGL = 0x8CEC;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CED.
        /// </summary>
        public const uint COLOR_ATTACHMENT13_WEBGL = 0x8CED;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CEE.
        /// </summary>
        public const uint COLOR_ATTACHMENT14_WEBGL = 0x8CEE;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CEF.
        /// </summary>
        public const uint COLOR_ATTACHMENT15_WEBGL = 0x8CEF;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8825.
        /// </summary>
        public const uint DRAW_BUFFER0_WEBGL = 0x8825;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8826.
        /// </summary>
        public const uint DRAW_BUFFER1_WEBGL = 0x8826;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8827.
        /// </summary>
        public const uint DRAW_BUFFER2_WEBGL = 0x8827;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8828.
        /// </summary>
        public const uint DRAW_BUFFER3_WEBGL = 0x8828;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8829.
        /// </summary>
        public const uint DRAW_BUFFER4_WEBGL = 0x8829;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x882A.
        /// </summary>
        public const uint DRAW_BUFFER5_WEBGL = 0x882A;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x882B.
        /// </summary>
        public const uint DRAW_BUFFER6_WEBGL = 0x882B;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x882C.
        /// </summary>
        public const uint DRAW_BUFFER7_WEBGL = 0x882C;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x882D.
        /// </summary>
        public const uint DRAW_BUFFER8_WEBGL = 0x882D;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x882E.
        /// </summary>
        public const uint DRAW_BUFFER9_WEBGL = 0x882E;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x882F.
        /// </summary>
        public const uint DRAW_BUFFER10_WEBGL = 0x882F;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8830.
        /// </summary>
        public const uint DRAW_BUFFER11_WEBGL = 0x8830;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8831.
        /// </summary>
        public const uint DRAW_BUFFER12_WEBGL = 0x8831;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8832.
        /// </summary>
        public const uint DRAW_BUFFER13_WEBGL = 0x8832;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8833.
        /// </summary>
        public const uint DRAW_BUFFER14_WEBGL = 0x8833;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8834.
        /// </summary>
        public const uint DRAW_BUFFER15_WEBGL = 0x8834;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8CDF.
        /// </summary>
        public const uint MAX_COLOR_ATTACHMENTS_WEBGL = 0x8CDF;
        /// <summary>
        /// WEBGL_draw_buffers. Value 0x8824.
        /// </summary>
        public const uint MAX_DRAW_BUFFERS_WEBGL = 0x8824;
        #endregion

        #region OES_vertex_array_object
        /// <summary>
        /// OES_vertex_array_object. Value 0x85B5.
        /// </summary>
        public const uint VERTEX_ARRAY_BINDING_OES = 0x85B5;
        #endregion

        #region EXT_disjoint_timer_query
        /// <summary>
        /// EXT_disjoint_timer_query. Value 0x8864.
        /// </summary>
        public const uint QUERY_COUNTER_BITS_EXT = 0x8864;
        /// <summary>
        /// EXT_disjoint_timer_query. Value 0x8865.
        /// </summary>
        public const uint CURRENT_QUERY_EXT = 0x8865;
        /// <summary>
        /// EXT_disjoint_timer_query. Value 0x8866.
        /// </summary>
        public const uint QUERY_RESULT_EXT = 0x8866;
        /// <summary>
        /// EXT_disjoint_timer_query. Value 0x8867.
        /// </summary>
        public const uint QUERY_RESULT_AVAILABLE_EXT = 0x8867;
        /// <summary>
        /// EXT_disjoint_timer_query. Value 0x88BF.
        /// </summary>
        public const uint TIME_ELAPSED_EXT = 0x88BF;
        /// <summary>
        /// EXT_disjoint_timer_query. Value 0x8E28.
        /// </summary>
        public const uint TIMESTAMP_EXT = 0x8E28;
        /// <summary>
        /// EXT_disjoint_timer_query. Value 0x8FBB.
        /// </summary>
        public const uint GPU_DISJOINT_EXT = 0x8FBB;
        #endregion
    }
}
