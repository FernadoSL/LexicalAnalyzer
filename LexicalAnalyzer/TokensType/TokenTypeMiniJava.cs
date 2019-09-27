using System.ComponentModel;

namespace AnalisadorLexico.TiposToken
{
    public enum TokenTypeMiniJava
    {
        [Description("class")]
        SCLASS,

        [Description("public")]
        SPUBLIC,

        [Description("static")]
        SSTATIC,

        [Description("void")]
        SVOID,

        [Description("main")]
        SMAIN,

        [Description("{")]
        SABRE_CHAVES,

        [Description("}")]
        SFECHA_CHAVES,

        [Description("(")]
        SABRE_PARENTESES,

        [Description(")")]
        SFECHA_PARENTESES,

        [Description("[")]
        SABRE_COLCHETES,

        [Description("]")]
        SFECHA_COLCHETES,

        [Description("extends")]
        SEXTENDS,

        [Description("int")]
        SINT,

        [Description(";")]
        SPONTO_VIRGULA,

        [Description("if")]
        SIF,

        [Description("else")]
        SELSE,

        [Description("while")]
        SWHILE,

        [Description("System.out.println")]
        SSYSTEM_OUT_PRINTLN,

        [Description("=")]
        SATRIBUICAO,

        [Description("&&")]
        SAND,

        [Description("<")]
        SMENOR,

        [Description("+")]
        SADICAO,

        [Description("-")]
        SSUBTRACAO,

        [Description("*")]
        SMULTIPLICACAO,

        [Description("true")]
        STRUE,

        [Description("false")]
        SFALSE,

        [Description("this")]
        STHIS,

        [Description("new")]
        SNEW,

        [Description("!")]
        SEXCLAMACAO
    }
}
