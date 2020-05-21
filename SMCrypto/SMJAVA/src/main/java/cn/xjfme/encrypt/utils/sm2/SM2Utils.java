package main.java.cn.xjfme.encrypt.utils.sm2;

import java.io.IOException;
import java.math.BigInteger;

import main.java.cn.xjfme.encrypt.utils.Util;
import org.bouncycastle.crypto.AsymmetricCipherKeyPair;
import org.bouncycastle.crypto.params.ECPrivateKeyParameters;
import org.bouncycastle.crypto.params.ECPublicKeyParameters;
import org.bouncycastle.math.ec.ECPoint;

public class SM2Utils
{
    //生成随机秘钥对
    public static void generateKeyPair(){
        SM2 sm2 = SM2.Instance();
        AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.generateKeyPair();
        ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters) key.getPrivate();
        ECPublicKeyParameters ecpub = (ECPublicKeyParameters) key.getPublic();
        BigInteger privateKey = ecpriv.getD();
        ECPoint publicKey = ecpub.getQ();

        System.out.println("公钥: " + Util.byteToHex(publicKey.getEncoded()));
        System.out.println("私钥: " + Util.byteToHex(privateKey.toByteArray()));
    }
    public static String encrypt(String publicKey, String plainText) throws IOException
    {
        if (publicKey == null || publicKey.length() == 0)
        {
            return null;
        }

        if (plainText == null || plainText.length() == 0)
        {
            return null;
        }
        byte[] sourceData = plainText.getBytes();
        String cipherText = encrypt(Util.hexToByte(publicKey), sourceData);
        return cipherText;
    }
    //数据加密
    public static String encrypt(byte[] publicKey, byte[] data) throws IOException
    {
        if (publicKey == null || publicKey.length == 0)
        {
            return null;
        }

        if (data == null || data.length == 0)
        {
            return null;
        }

        byte[] source = new byte[data.length];
        System.arraycopy(data, 0, source, 0, data.length);

        Cipher cipher = new Cipher();
        SM2 sm2 = SM2.Instance();
        ECPoint userKey = sm2.ecc_curve.decodePoint(publicKey);

        ECPoint c1 = cipher.Init_enc(sm2, userKey);
        cipher.Encrypt(source);
        byte[] c3 = new byte[32];
        cipher.Dofinal(c3);

        String sc1 = Util.byteToHex(c1.getEncoded());
        String sc2 = Util.byteToHex(source);
        String sc3 = Util.byteToHex(c3);
		//System.out.println("C1 " + sc1);
		//System.out.println("C2 " + sc2);
		//System.out.println("C3 " + sc3);
        //C1 C2 C3拼装成加密字串
        return (sc1 + sc2+ sc3).toUpperCase();

    }

    public static String decrypt(String privateKey, String cipherText) throws IOException {
        if (privateKey == null || privateKey.length() == 0) {
            return null;
        }

        if (cipherText == null || cipherText.length() == 0) {
            return null;
        }
        String plainText = new String(decrypt(Util.hexToByte(privateKey), Util.hexToByte(cipherText)),"UTF8");
        return plainText;
    }
    //数据解密
    public static byte[] decrypt(byte[] privateKey, byte[] encryptedData) throws IOException
    {
        if (privateKey == null || privateKey.length == 0)
        {
            return null;
        }

        if (encryptedData == null || encryptedData.length == 0)
        {
            return null;
        }
        //加密字节数组转换为十六进制的字符串 长度变为encryptedData.length * 2
        String data = Util.byteToHex(encryptedData);
        /***分解加密字串
         * （C1 = C1标志位2位 + C1实体部分128位 = 130）
         * （C3 = C3实体部分64位  = 64）
         * （C2 = encryptedData.length * 2 - C1长度  - C2长度）
         */
        byte[] c1Bytes = Util.hexToByte(data.substring(0,130));
        int c2Len = encryptedData.length - 97;
        byte[] c2 = Util.hexToByte(data.substring(130,130 + 2 * c2Len));
        byte[] c3 = Util.hexToByte(data.substring(130 + 2 * c2Len,194 + 2 * c2Len));

        SM2 sm2 = SM2.Instance();
        BigInteger userD = new BigInteger(1, privateKey);

        //通过C1实体字节来生成ECPoint
        ECPoint c1 = sm2.ecc_curve.decodePoint(c1Bytes);
        Cipher cipher = new Cipher();
        cipher.Init_dec(userD, c1);
        cipher.Decrypt(c2);
        cipher.Dofinal(c3);

        //返回解密结果
        return c2;
    }

    public static void testMain(String plainText) throws Exception
    {
        System.out.println("\n--测试SM2加密/解密--");
        //生成密钥对
        //generateKeyPair();

        //String plainText = "ererfeiisgod";
       // byte[] sourceData = plainText.getBytes();

        //下面的秘钥可以使用generateKeyPair()生成的秘钥内容
        // 国密规范正式私钥
        String prik = "3260bde898927b8300619422b53ebbcf0093750baf452a6bad3c47b1c5033f8d";
        // 国密规范正式公钥
        String pubk = "044cc83d9bedf7e4fcdcf81aa7aac97a6fc299c64f21aca5c6a648111efa6bd318df5fe176bc8becf60345ac9708e970b051d9003dce38dd90e15ea93dc46cf166";
        System.out.println("SM2公钥: " + pubk);
        System.out.println("SM2私钥: " + prik);

        String cipherText = SM2Utils.encrypt(pubk, plainText);
        System.out.println("SM2加密: "+cipherText);
        //cipherText="04F5B42C3CF241E45A3D4884395C0B5EB2BA9EE4D9CF3B90FF5F9364A1B5657614E3643A241541E0DC208FFC98D73DC5BB9668DFCECE7D653B7CA49163E887668CAF5560F29FC94D493A7D2587E018925563A3F883024A925D36B9E92B2E9467B4A9EB12B712E68414B65A1D6E5F0FEE16C52DCF34BAFA3EA4830C715DA4CEDF141361707B83570C13527E4D52EA";
        plainText = SM2Utils.decrypt(prik, cipherText);
        System.out.println("SM2解密: "+plainText);
        System.out.println("--测试SM2加密/解密结束--");
    }
}