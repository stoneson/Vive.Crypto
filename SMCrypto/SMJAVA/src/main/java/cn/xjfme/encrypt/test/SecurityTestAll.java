package main.java.cn.xjfme.encrypt.test;
import main.java.cn.xjfme.encrypt.utils.Util;
import main.java.cn.xjfme.encrypt.utils.sm2.SM2EncDecUtils;
import main.java.cn.xjfme.encrypt.utils.sm2.SM2KeyVO;
import main.java.cn.xjfme.encrypt.utils.sm2.SM2SignVO;
import main.java.cn.xjfme.encrypt.utils.sm2.SM2Utils;
import main.java.cn.xjfme.encrypt.utils.sm4.SM4JSUtils;
import main.java.cn.xjfme.encrypt.utils.sm4.SM4Utils;
import org.bouncycastle.crypto.digests.SM3Digest;
import org.bouncycastle.util.encoders.Hex;

import java.util.UUID;

public class SecurityTestAll {

    public static void main(String[] args) throws Exception {
        //System.out.println("--生成SM4秘钥--");
        String sm4Key = "qawsedrftgyhujik";//SM4Utils.generateSM4Key();
        String src = "加密和解密都是用C#就可以完美的解决了。";
        System.out.println("SM4秘钥:"+sm4Key);
       // System.out.println("--生成SM4结束--");
        System.out.println("SM4明文:"+src);
        //--------------------------------------------------
        System.out.println("--SM4js的CBC加密--");
        String s1 = SM4JSUtils.SM4EncForCBC(sm4Key, src);
        System.out.println("密文:"+s1);
        System.out.println("CBC解密");
        String s2 = SM4JSUtils.SM4DecForCBC(sm4Key, s1);
        System.out.println("解密结果:"+s2);

        //System.out.println("--ECBjs加密--");
        String s3 = SM4JSUtils.SM4EncForECB(sm4Key, src,false);
        System.out.println("SM4js-ECB加密结果:"+s3);
        //System.out.println("--ECB解密--");
        //s3="0f8cd3dea53d5f404f9e1268ab45dd75ea96c4d15db7c2ddefbbc670c8bd5d3a3989e48068c85645f67510f41cc013f9";
        String s4 = SM4JSUtils.SM4DecForECB(sm4Key, s3,false);
        System.out.println("SM4js-ECB解密结果:"+s4);
        //--------------------------------------------------
        System.out.println("--SM4的CBC加密--");
         s1 = SM4Utils.SM4EncForCBC(sm4Key, src);
        System.out.println("密文:"+s1);
        System.out.println("CBC解密");
         s2 = SM4Utils.SM4DecForCBC(sm4Key, s1);
        System.out.println("解密结果:"+s2);

        //System.out.println("--ECB加密--");
         s3 = SM4Utils.SM4EncForECB(sm4Key, src,false);
        System.out.println("SM4-ECB加密结果:"+s3);
        //System.out.println("--ECB解密--");
        //s3="0f8cd3dea53d5f404f9e1268ab45dd75ea96c4d15db7c2ddefbbc670c8bd5d3a3989e48068c85645f67510f41cc013f9";
         s4 = SM4Utils.SM4DecForECB(sm4Key, s3,false);
        System.out.println("SM4-ECB解密结果:"+s4);
//--------------------------------------------------------------
        System.out.println("--SM3摘要测试--");
        String s = generateSM3HASH(src);
        System.out.println("hash:"+s);
        System.out.println("--SM3摘要结束--");
        //------------------------------------------------------------------------
        // 国密规范正式私钥
        String prik = "3260bde898927b8300619422b53ebbcf0093750baf452a6bad3c47b1c5033f8d";
        // 国密规范正式公钥
        String pubk = "044cc83d9bedf7e4fcdcf81aa7aac97a6fc299c64f21aca5c6a648111efa6bd318df5fe176bc8becf60345ac9708e970b051d9003dce38dd90e15ea93dc46cf166";
        System.out.println("\n--产生SM2秘钥--:");
        /*SM2KeyVO sm2KeyVO = SM2EncDecUtils.generateKeyPair();
        pubk=sm2KeyVO.getPubHexInSoft();
        prik=sm2KeyVO.getPriHexInSoft();
         */
        System.out.println("SM2公钥:" + pubk);
        System.out.println("SM2私钥:" + prik);
        //数据加密
        System.out.println("\n--测试SM2加密开始--");
        //String src = "I Love You";
        System.out.println("原文UTF-8转hex:" + Util.byteToHex(src.getBytes()));
        String SM2Enc = SM2EncDecUtils.encrypt(pubk, src);
        System.out.println("加密:");
        System.out.println("密文:" + SM2Enc);
        String SM2Dec = SM2EncDecUtils.decrypt(prik, SM2Enc);
        System.out.println("解密:" + SM2Dec);
        System.out.println("--测试加密结束--");
//--------------------------------------------------------------------
        System.out.println("\n--测试SM2签名--");
        System.out.println("原文hex:" + Util.byteToHex(src.getBytes()));
        String s5 = Util.byteToHex(src.getBytes());

        System.out.println("签名测试开始:");
        SM2SignVO sign = SM2EncDecUtils.genSM2Signature(prik, s5);
        System.out.println("软加密签名结果:" + sign.getSm2_signForSoft());
        System.out.println("加密机签名结果:" + sign.getSm2_signForHard());
        //System.out.println("转签名测试:"+SM2SignHardToSoft(sign.getSm2_signForHard()));
        System.out.println("验签1,软件加密方式:");
        boolean b = SM2EncDecUtils.verifySM2Signature(pubk, s5, sign.getSm2_signForSoft());
        System.out.println("软件加密方式验签结果:" + b);
        System.out.println("验签2,硬件加密方式:");
        String sm2_signForHard = sign.getSm2_signForHard();
        System.out.println("签名R:"+sign.sign_r);
        System.out.println("签名S:"+sign.sign_s);
        //System.out.println("硬:"+sm2_signForHard);
        b = SM2EncDecUtils.verifySM2Signature(pubk, s5, SM2EncDecUtils.SM2SignHardToSoft(sign.getSm2_signForHard()));
        System.out.println("硬件加密方式验签结果:" + b);
        if (!b) {
            throw new RuntimeException();
        }
        System.out.println("--签名测试结束--");



        //SM2Utils.testMain(src);
    }

    //摘要计算
    public static String generateSM3HASH(String src) {
        byte[] md = new byte[32];
        byte[] msg1 = src.getBytes();
        //System.out.println(Util.byteToHex(msg1));
        org.bouncycastle.crypto.digests.SM3Digest sm3 = new org.bouncycastle.crypto.digests.SM3Digest();
        sm3.update(msg1, 0, msg1.length);
        sm3.doFinal(md, 0);
        String s = new String(org.bouncycastle.util.encoders.Hex.encode(md));
        return s.toUpperCase();
    }
}
