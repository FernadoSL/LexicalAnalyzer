using System.ComponentModel;

namespace AnalisadorLexico
{
    public enum TokenTypeLdp
    {
        [Description("programa")]
        SPROGRAMA,          //programa

        [Description("var")]
        SVAR,               //var

        [Description(":")]
        SDOISPONTOS,            //:

        [Description("inicio")]
        SINICIO,                //inicio

        [Description("fim")]
        SFIM,               //fim

        [Description(":=")]
        SATRIBUICAO,            //:=

        //[Description(":")]
        //STIPO,              //:

        [Description("escreva")]
        SESCREVA,               //escreva

        [Description("inteiro")]
        SINTEIRO,               //inteiro

        [Description(";")]
        SPONTO_E_VIRGULA,       //;

        [Description(".")]
        SPONTO,             //.

        [Description("+")]
        SMAIS,              //+

        [Description("-")]
        SMENOS,             //-

        [Description("*")]
        SMULTIPLICACAO,         //*

        [Description("numero")]
        SNUMERO,                //5

        [Description("identificador")]
        SIDENTIFICADOR,         //x, teste

        [Description("(")]
        SABRE_PARENTESIS,       //(

        [Description(")")]
        SFECHA_PARENTESIS,      //)

        SERRO                   //Usado para tokens não reconhecidos
    }
}
