using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLayer.Models.Utility
{
    public class EncodeData
    {

        //public string PassEncode1(string Password)
        //{
        //    string Pas, NewPas1, NewPas2 = string.Empty;
        //    int Cnt, Cnt1, Cnt2;

        //    Pas = Password;
        //    Cnt1 = 100;
        //    Cnt2 = 70;

        //    for (Cnt = 1; Cnt < Pas.Length; Cnt++)
        //    {

        //        Char.ConvertFromUtf32(Pas[Cnt])
        //        NewPas1 = IntToStr(Ord(Pas[Cnt]));
        //        if (Cnt < 4)
        //        {
        //            NewPas1 = IntToStr(StrToInt(NewPas1) + Cnt1);
        //            Cnt1 = Cnt1 - 10;
        //        }
        //        else
        //        {
        //            NewPas1 = IntToStr(StrToInt(NewPas1) + Cnt2);
        //        }
        //    }
        //    NewPas2 = NewPas1 + NewPas2;

        //    return NewPas2;
        //}

        //public string PassEncode2(string Password)
        //{
        //    string Pas = string.Empty;
        //    string NewPas2 = string.Empty;
        //    string NewPas3 = string.Empty;
        //    string NewPas4 = string.Empty;

        //    int Cnt, Cnt1, Cnt2, i, j = 0;

        //    i = 1;
        //    j = 3;
        //    Pas = Password;
        //    Cnt1 = Pas.Length % 3;

        //    for (Cnt = 1; Cnt <= Cnt1; Cnt++)
        //    {
        //        for (Cnt2 = 1; Cnt <= j; Cnt2++)
        //        {
        //            NewPas3 = Pas[Cnt2];
        //            NewPas2 = NewPas2 + NewPas3;
        //        }
        //        NewPas4 = NewPas4 + Chr(StrToInt(NewPas2));
        //        i = i + 3;
        //        j = j + 3;
        //        NewPas2 = "";
        //        NewPas3 = "";
        //    }
        //    return NewPas4;
        //}
        //public string Jumble(string Password)
        //{
        //    string JumPass = string.Empty;
        //    string JumbledPass = string.Empty;
        //    string FirstSeq = string.Empty;
        //    string FirstSeq1 = string.Empty;
        //    string SecondSeq = string.Empty;
        //    string SecondSeq1 = string.Empty;

        //    int FirstCnt, Cnt1, i, j = 0;

        //    JumPass = Password;
        //    j = JumPass.Length;
        //    Cnt1 = JumPass.Length % 3;


        //    for (FirstCnt = 1; FirstCnt < Cnt1; FirstCnt++)
        //    {
        //        for (i = 1; FirstCnt <= 3; i++)
        //        {
        //            FirstSeq1 = JumPass[j].ToString();
        //            j = j - 1;
        //        }

        //        SecondSeq1 = FirstSeq;
        //        SecondSeq = SecondSeq + SecondSeq1;
        //        FirstSeq = "";
        //        JumbledPass = SecondSeq;
        //    }
        //    return JumbledPass;

        //}

    }
}