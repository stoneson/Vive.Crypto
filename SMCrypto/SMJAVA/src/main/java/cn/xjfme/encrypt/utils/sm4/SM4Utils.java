package main.java.cn.xjfme.encrypt.utils.sm4;

/**
 * Created by $(USER) on $(DATE)
 */

import main.java.cn.xjfme.encrypt.utils.Util;
import org.apache.commons.codec.binary.Base64;
import sun.misc.BASE64Decoder;
import sun.misc.BASE64Encoder;

import java.io.IOException;
import java.util.UUID;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class SM4Utils {
    public String secretKey = "";
    public String iv = "";
    public boolean hexString = false;

    public SM4Utils() {
    }

    public String encryptData_ECB(String plainText)
    {
        try
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_ENCRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Util.hexStringToBytes(secretKey);
            }
            else
            {
                keyBytes = secretKey.getBytes();
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_ecb(ctx, plainText.getBytes("UTF8"));
            return  new String(org.bouncycastle.util.encoders.Hex.encode(encrypted), "UTF-8");
        }
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
    public String decryptData_ECB(String cipherText)
    {
        try
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_DECRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Util.hexStringToBytes(secretKey);
            }
            else
            {
                keyBytes = secretKey.getBytes();
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_ecb(ctx, org.bouncycastle.util.encoders.Hex.decode(cipherText));
            return new String(decrypted, "UTF8");
        }
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }

    public String encryptData_CBC(String plainText)
    {
        try
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_ENCRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Util.hexStringToBytes(secretKey);
                ivBytes = Util.hexStringToBytes(iv);
            }
            else
            {
                keyBytes = secretKey.getBytes();
                ivBytes = iv.getBytes();
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, plainText.getBytes("UTF8"));
            return  new String(org.bouncycastle.util.encoders.Hex.encode(encrypted), "UTF-8");
        }
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }

    public String decryptData_CBC(String cipherText)
    {
        try
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_DECRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Util.hexStringToBytes(secretKey);
                ivBytes = Util.hexStringToBytes(iv);
            }
            else
            {
                keyBytes = secretKey.getBytes();
                ivBytes = iv.getBytes();
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, org.bouncycastle.util.encoders.Hex.decode(cipherText));
           return new String(decrypted, "UTF8");
        }
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }

    //产生对称秘钥
    public static String generateSM4Key() {
        return UUID.randomUUID().toString().replace("-", "");
    }

    //对称秘钥加密(ECB)
    public static String SM4EncForECB(String key,String text,boolean hexString) {
        SM4Utils sm4 = new SM4Utils();
        sm4.secretKey = key;
        sm4.hexString = hexString;
        String cipherText = sm4.encryptData_ECB(text);
        return cipherText;
    }
    //对称秘钥解密(ECB)
    public static String SM4DecForECB(String key,String text,boolean hexString) {
        SM4Utils sm4 = new SM4Utils();
        sm4.secretKey = key;
        sm4.hexString = hexString;
        String plainText = sm4.decryptData_ECB(text);
        return plainText;
    }

    //对称秘钥加密(CBC)
    public static String SM4EncForCBC(String key,String text) {
        SM4Utils sm4 = new SM4Utils();
        sm4.secretKey = key;
        sm4.hexString = false;
        sm4.iv = "41036be33171466e9114907562bc70e6";
        String cipherText = sm4.encryptData_CBC(text);
        return cipherText;
    }

    //对称秘钥解密(CBC)
    public static String SM4DecForCBC(String key,String text) {
        SM4Utils sm4 = new SM4Utils();
        sm4.secretKey = key;
        sm4.hexString = false;
        sm4.iv = "41036be33171466e9114907562bc70e6";
        String plainText = sm4.decryptData_CBC(text);
        return plainText;
    }

/*
    public static void main(String[] args) throws IOException {
        String plainText = "I Love You Every Day";
        String s = Util.byteToHex(plainText.getBytes());
        System.out.println("原文" + s);
        SM4Utils sm4 = new SM4Utils();
        //sm4.secretKey = "JeF8U9wHFOMfs2Y8";
        sm4.secretKey = "64EC7C763AB7BF64E2D75FF83A319918";
        sm4.hexString = true;

        System.out.println("ECB模式加密");
        String cipherText = sm4.encryptData_ECB(plainText);
        System.out.println("密文: " + cipherText);
        System.out.println("");

        String plainText2 = sm4.decryptData_ECB(cipherText);
        System.out.println("明文: " + plainText2);
        System.out.println("");

        System.out.println("CBC模式加密");
        sm4.iv = "31313131313131313131313131313131";
        String cipherText2 = sm4.encryptData_CBC(plainText);
        System.out.println("加密密文: " + cipherText2);
        System.out.println("");

        String plainText3 = sm4.decryptData_CBC(cipherText2);
        System.out.println("解密明文: " + plainText3);

    }

 */
}
