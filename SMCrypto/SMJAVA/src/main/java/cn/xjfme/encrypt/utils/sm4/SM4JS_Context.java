package main.java.cn.xjfme.encrypt.utils.sm4;

/**
 * Created by $(USER) on $(DATE)
 */
public class SM4JS_Context {
    public int mode;

    public int[] sk;

    public boolean isPadding;

    public SM4JS_Context()
    {
        this.mode = 1;
        this.isPadding = true;
        this.sk = new int[32];
    }
}

