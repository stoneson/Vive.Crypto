function Hex () {

}

Hex.encode = function (b, pos, len) {
  var hexCh = new Array(len * 2);
  var hexCode = new Array('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f');

  for (var i = pos, j = 0; i < len + pos; i++, j++) {
    var v = b[i] & 255;
    hexCh[j] = hexCode[v >>> 4];
    hexCh[++j] = hexCode[v & 15];
  }

  return hexCh.join('');
}

Hex.decode = function (hex) {

  if (hex == null || hex == '') {
    return null;
  }
  if (hex.length % 2 != 0) {
    return null;
  }

  var ascLen = hex.length / 2;
  var hexCh = this.toCharCodeArray(hex);
  var asc = new Array(ascLen);

  for (var i = 0; i < ascLen; i++) {

    if (hexCh[2 * i] >= 0x30 && hexCh[2 * i] <= 0x39) {
      asc[i] = ((hexCh[2 * i] - 0x30) << 4);
    } else if (hexCh[2 * i] >= 0x41 && hexCh[2 * i] <= 0x46) {//A-F : 0x41-0x46
      asc[i] = ((hexCh[2 * i] - 0x41 + 10) << 4);
    } else if (hexCh[2 * i] >= 0x61 && hexCh[2 * i] <= 0x66) {//a-f  : 0x61-0x66
      asc[i] = ((hexCh[2 * i] - 0x61 + 10) << 4);
    } else {
      return null;
    }

    if (hexCh[2 * i + 1] >= 0x30 && hexCh[2 * i + 1] <= 0x39) {
      asc[i] = (asc[i] | (hexCh[2 * i + 1] - 0x30));
    } else if (hexCh[2 * i + 1] >= 0x41 && hexCh[2 * i + 1] <= 0x46) {
      asc[i] = (asc[i] | (hexCh[2 * i + 1] - 0x41 + 10));
    } else if (hexCh[2 * i + 1] >= 0x61 && hexCh[2 * i + 1] <= 0x66) {
      asc[i] = (asc[i] | (hexCh[2 * i + 1] - 0x61 + 10));
    } else {
      return null;
    }


  }

  return asc;
}

Hex.utf8StrToHex = function (utf8Str) {
  var ens = encodeURIComponent(utf8Str);
  var es = unescape(ens);


  var esLen = es.length;

  // Convert
  var words = [];
  for (var i = 0; i < esLen; i++) {
    words[i] = (es.charCodeAt(i).toString(16));
  }
  return words.join('');
}

Hex.utf8StrToBytes = function (utf8Str) {
  var ens = encodeURIComponent(utf8Str);
  var es = unescape(ens);


  var esLen = es.length;

  // Convert
  var words = [];
  for (var i = 0; i < esLen; i++) {
    words[i] = es.charCodeAt(i);
  }
  return words;
}

Hex.hexToUtf8Str = function (utf8Str) {

  var utf8Byte = Hex.decode(utf8Str);
  var latin1Chars = [];
  for (var i = 0; i < utf8Byte.length; i++) {
    latin1Chars.push(String.fromCharCode(utf8Byte[i]));
  }
  return decodeURIComponent(escape(latin1Chars.join('')));
}

Hex.bytesToUtf8Str = function (bytesArray) {

  var utf8Byte = bytesArray;
  var latin1Chars = [];
  for (var i = 0; i < utf8Byte.length; i++) {
    latin1Chars.push(String.fromCharCode(utf8Byte[i]));
  }
  return decodeURIComponent(escape(latin1Chars.join('')));
}

Hex.toCharCodeArray = function (chs) {
  var chArr = new Array(chs.length);
  for (var i = 0; i < chs.length; i++) {
    chArr[i] = chs.charCodeAt(i);
  }
  return chArr;
}

//===========================================================================================================
//byte&string.js
function isNull (obj) {
  if (obj == undefined || obj == null || obj == '') {
    return true;
  }
  for (var a in obj) {
    return false;
  }
  return ture;
}

function stringToByte (str) {
  var bytes = new Array();
  //alert('bytes:' + bytes.length);
  var len, c;
  len = str.length;
  for (var i = 0; i < len; i++) {
    c = str.charCodeAt(i);
    if (c >= 0x010000 && c <= 0x10FFFF) {
      bytes.push(((c >> 18) & 0x07) | 0xF0);
      bytes.push(((c >> 12) & 0x3F) | 0x80);
      bytes.push(((c >> 6) & 0x3F) | 0x80);
      bytes.push((c & 0x3F) | 0x80);
    } else if (c >= 0x000800 && c <= 0x00FFFF) {
      bytes.push(((c >> 12) & 0x0F) | 0xE0);
      bytes.push(((c >> 6) & 0x3F) | 0x80);
      bytes.push((c & 0x3F) | 0x80);
    } else if (c >= 0x000080 && c <= 0x0007FF) {
      bytes.push(((c >> 6) & 0x1F) | 0xC0);
      bytes.push((c & 0x3F) | 0x80);
    } else {
      bytes.push(c & 0xFF);
    }
  }
  // alert('bytes:' + int32View.length);
  return bytes;


}


function byteToString (arr) {
  if (typeof arr === 'string') {
    return arr;
  }
  var str = '',
    _arr = arr;
  for (var i = 0; i < _arr.length; i++) {
    var one = _arr[i].toString(2),
      v = one.match(/^1+?(?=0)/);
    if (v && one.length == 8) {
      var bytesLength = v[0].length;
      var store = _arr[i].toString(2).slice(7 - bytesLength);
      for (var st = 1; st < bytesLength; st++) {
        store += _arr[st + i].toString(2).slice(2);
      }
      str += String.fromCharCode(parseInt(store, 2));
      i += bytesLength - 1;
    } else {
      str += String.fromCharCode(_arr[i]);
    }
  }
  return str;
}
//===========================================================================================================
/*
 * 
 * 字节流转换工具js
 * 
 */

/*
 * 数组复制
 */
function arrayCopy (src, pos1, dest, pos2, len) {
  var realLen = len;
  if (pos1 + len > src.length && pos2 + len <= dest.length) {
    realLen = src.length - pos1;
  } else if (pos2 + len > dest.length && pos1 + len <= src.length) {
    realLen = dest.length - pos2;
  } else if (pos1 + len <= src.length && pos2 + len <= dest.length) {
    realLen = len;
  } else if (dest.length < src.length) {
    realLen = dest.length - pos2;
  } else {
    realLen = src.length - pos2;
  }

  for (var i = 0; i < realLen; i++) {
    dest[i + pos2] = src[i + pos1];
  }
}

/*
 * 长整型转成字节，一个长整型为8字节
 * 返回：字节数组
 */
function longToByte (num) {
  //TODO 这里目前只转换了低四字节，因为js没有长整型，得要封装
  return new Array(
    0,
    0,
    0,
    0,
    (num >> 24) & 0x000000FF,
    (num >> 16) & 0x000000FF,
    (num >> 8) & 0x000000FF,
    (num) & 0x000000FF
  );
}

/*
 * int数转成byte数组
 * 事实上只不过转成byte大小的数，实际占用空间还是4字节
 * 返回：字节数组
 */
function intToByte (num) {
  return new Array(
    (num >> 24) & 0x000000FF,
    (num >> 16) & 0x000000FF,
    (num >> 8) & 0x000000FF,
    (num) & 0x000000FF
  );
}

/*
 * int数组转成byte数组，一个int数值转成四个byte
 * 返回:byte数组
 */
function intArrayToByteArray (nums) {
  var b = new Array(nums.length * 4);

  for (var i = 0; i < nums.length; i++) {
    arrayCopy(intToByte(nums[i]), 0, b, i * 4, 4);
  }

  return b;
}

/*
 * byte数组转成int数值
 * 返回：int数值
 */
function byteToInt (b, pos) {
  if (pos + 3 < b.length) {
    return ((b[pos]) << 24) | ((b[pos + 1]) << 16) | ((b[pos + 2]) << 8) | ((b[pos + 3]));
  } else if (pos + 2 < b.length) {
    return ((b[pos + 1]) << 16) | ((b[pos + 2]) << 8) | ((b[pos + 3]));
  } else if (pos + 1 < b.length) {
    return ((b[pos]) << 8) | ((b[pos + 1]));
  } else {
    return ((b[pos]));
  }
}

/*
 * byte数组转成int数组,每四个字节转成一个int数值
 * 
 */
function byteArrayToIntArray (b) {
  // var arrLen = b.length%4==0 ? b.length/4:b.length/4+1;
  var arrLen = Math.ceil(b.length / 4);//向上取整
  var out = new Array(arrLen);
  for (var i = 0; i < b.length; i++) {
    b[i] = b[i] & 0xFF;//避免负数造成影响
  }
  for (var i = 0; i < out.length; i++) {
    out[i] = byteToInt(b, i * 4);
  }
  return out;
}

function getHeight4 (ata) {//获取高四位
  var height = ((data & 0xf0) >> 4);
  return height;
}

function getLow4 (data) {//获取低四位
  var low = (data & 0x0f);
  return low;
}
//===========================================================================================================


/**
 * base64js
 */
/**
 * base64js
 * base64js.toByteArray(d.input)
 * base64js.fromByteArray(c);
 * @author c.z.s
 * @email 1048829253@qq.com
 * @company
 * @date 2018-07
 *
 */
(function (r) {
  if (typeof exports === "object" && typeof module !== "undefined") {
    module.exports = r()
  } else {
    if (typeof define ===
      "function" && define.amd) {
      define([], r)
    } else {
      var e;
      if (typeof window !== "undefined") {
        e = window
      } else {
        if (typeof global
          !== "undefined") {
          e = global
        } else {
          if (typeof self !== "undefined") {
            e = self
          } else {
            e = this
          }
        }
      }
      e.base64js = r()
    }
  }
})(function () {
  var r, e, t;
  return function r (e, t, n) {
    function o (i, a) {
      if (!t[i]) {
        if (!e[i]) {
          var u = typeof require == "function" && require;
          if (!a && u) {
            return u(i, !0)
          }
          if (f) {
            return f(i, !0)
          }
          var d = new Error("Cannot find module '" + i + "'");
          throw d.code = "MODULE_NOT_FOUND", d
        }
        var c = t[i] = { exports: {} };
        e[i][0].call(c.exports, function (r) {
          var t = e[i][1][r];
          return o(t ? t : r)
        }, c, c.exports, r, e, t, n)
      }
      return t[i].exports
    }

    var f = typeof require == "function" && require;
    for (var i = 0; i < n.length; i++) {
      o(n[i])
    }
    return o
  }({
    "/": [function (r, e, t) {
      t.byteLength = c;
      t.toByteArray = v;
      t.fromByteArray = s;
      var n = [];
      var o = [];
      var f = typeof Uint8Array !== "undefined" ? Uint8Array : Array;
      var i = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
      for (var a = 0, u = i.length; a < u; ++a) {
        n[a] = i[a];
        o[i.charCodeAt(a)] = a
      }
      o["-".charCodeAt(0)] = 62;
      o["_".charCodeAt(0)] = 63;

      function d (r) {
        var e = r.length;
        if (e % 4 > 0) {
          throw new Error("Invalid string. Length must be a multiple of 4")
        }
        return r[e - 2] === "=" ? 2 : r[e - 1] === "=" ? 1 : 0
      }

      function c (r) {
        return r.length * 3 / 4 - d(r)
      }

      function v (r) {
        var e, t, n, i, a;
        var u = r.length;
        i = d(r);
        a = new f(u * 3 / 4 - i);
        t = i > 0 ? u - 4 : u;
        var c = 0;
        for (e = 0; e < t; e += 4) {
          n = o[r.charCodeAt(e)] << 18 | o[r.charCodeAt(e + 1)] << 12 | o[r.charCodeAt(e + 2)] << 6 | o[r.charCodeAt(e + 3)];
          a[c++] = n >> 16 & 255;
          a[c++] = n >> 8 & 255;
          a[c++] = n & 255
        }
        if (i === 2) {
          n = o[r.charCodeAt(e)] << 2 | o[r.charCodeAt(e + 1)] >> 4;
          a[c++] = n & 255
        } else {
          if (i === 1) {
            n = o[r.charCodeAt(e)] << 10 | o[r.charCodeAt(e + 1)] << 4 | o[r.charCodeAt(e + 2)] >> 2;
            a[c++] = n >> 8 & 255;
            a[c++] = n & 255
          }
        }
        return a
      }

      function l (r) {
        return n[r >> 18 & 63] + n[r >> 12 & 63] + n[r >> 6 & 63] + n[r & 63]
      }

      function h (r, e, t) {
        var n;
        var o = [];
        for (var f = e; f < t; f += 3) {
          n = (r[f] << 16) + (r[f + 1] << 8) + r[f + 2];
          o.push(l(n))
        }
        return o.join("")
      }

      function s (r) {
        var e;
        var t = r.length;
        var o = t % 3;
        var f = "";
        var i = [];
        var a = 16383;
        for (var u = 0, d = t - o; u < d; u += a) {
          i.push(h(r, u, u + a > d ? d : u + a))
        }
        if (o === 1) {
          e = r[t - 1];
          f += n[e >> 2];
          f += n[e << 4 & 63];
          f += "=="
        } else {
          if (o === 2) {
            e = (r[t - 2] << 8) + r[t - 1];
            f += n[e >> 10];
            f += n[e >> 4 & 63];
            f += n[e << 2 & 63];
            f += "="
          }
        }
        i.push(f);
        return i.join("")
      }
    }, {}]
  }, {}, [])("/")
});
//===========================================================================================================
//封装sm4.js，实现ECB工作模式
function context () {
  this.mode = 1;
  this.sk = new Array(32);
  this.isPadding = true;
}
function bytesToLong (array, i) {
  if (array.length === 0) return 0
  return (((array[i + 0] & 0xff) << 56)
    | ((array[i + 1] & 0xff) << 48)
    | ((array[i + 2] & 0xff) << 40)
    | ((array[i + 3] & 0xff) << 32)
    | ((array[i + 4] & 0xff) << 24)
    | ((array[i + 5] & 0xff) << 16)
    | ((array[i + 6] & 0xff) << 8)
    | ((array[i + 7] & 0xff) << 0))
}
//byte转long
function GET_ULONG_BE (b, i) {
  //return bytesToLong(b, i);
  var n = (((b[i] & 0xff) << 24) | ((b[i + 1] & 0xff) << 16) | ((b[i + 2] & 0xff) << 8) | (b[i + 3] & 0xff)) & 0xffffffff;
  return (n);
};

//long转byte
function PUT_ULONG_BE (n, b, i) {
  n = (n);
  b[i] = (0xff & n >> 24);
  b[i + 1] = (0xff & n >> 16);
  b[i + 2] = (0xff & n >> 8);
  b[i + 3] = (0xff & n);
}

//左移n
function SHL (x, n) {
  return ((x & 0xFFFFFFFF) << n);
}

//循环左移n
function ROTL (x, n) {
  return SHL(x, n) | (x >> (32 - n));
}

//交换sk的i位和31-i位
function SWAP (sk, i) {
  t = (sk[i]);
  sk[i] = sk[(31 - i)];
  sk[(31 - i)] = t;
}

/**
 * int 转 byte
 * @param i 
 */
function int2Byte (i) {
  var b = i & 0xFF;
  var c = 0;
  if (b >= 128) {
    c = b % 128;
    c = -1 * (128 - c);
  } else {
    c = b;
  }
  //console.log(c)
  return c;
}

var sboxTable = [
  0xd6, 0x90, 0xe9, 0xfe, 0xcc, 0xe1, 0x3d, 0xb7, 0x16, 0xb6, 0x14, 0xc2, 0x28, 0xfb, 0x2c, 0x05,
  0x2b, 0x67, 0x9a, 0x76, 0x2a, 0xbe, 0x04, 0xc3, 0xaa, 0x44, 0x13, 0x26, 0x49, 0x86, 0x06, 0x99,
  0x9c, 0x42, 0x50, 0xf4, 0x91, 0xef, 0x98, 0x7a, 0x33, 0x54, 0x0b, 0x43, 0xed, 0xcf, 0xac, 0x62,
  0xe4, 0xb3, 0x1c, 0xa9, 0xc9, 0x08, 0xe8, 0x95, 0x80, 0xdf, 0x94, 0xfa, 0x75, 0x8f, 0x3f, 0xa6,
  0x47, 0x07, 0xa7, 0xfc, 0xf3, 0x73, 0x17, 0xba, 0x83, 0x59, 0x3c, 0x19, 0xe6, 0x85, 0x4f, 0xa8,
  0x68, 0x6b, 0x81, 0xb2, 0x71, 0x64, 0xda, 0x8b, 0xf8, 0xeb, 0x0f, 0x4b, 0x70, 0x56, 0x9d, 0x35,
  0x1e, 0x24, 0x0e, 0x5e, 0x63, 0x58, 0xd1, 0xa2, 0x25, 0x22, 0x7c, 0x3b, 0x01, 0x21, 0x78, 0x87,
  0xd4, 0x00, 0x46, 0x57, 0x9f, 0xd3, 0x27, 0x52, 0x4c, 0x36, 0x02, 0xe7, 0xa0, 0xc4, 0xc8, 0x9e,
  0xea, 0xbf, 0x8a, 0xd2, 0x40, 0xc7, 0x38, 0xb5, 0xa3, 0xf7, 0xf2, 0xce, 0xf9, 0x61, 0x15, 0xa1,
  0xe0, 0xae, 0x5d, 0xa4, 0x9b, 0x34, 0x1a, 0x55, 0xad, 0x93, 0x32, 0x30, 0xf5, 0x8c, 0xb1, 0xe3,
  0x1d, 0xf6, 0xe2, 0x2e, 0x82, 0x66, 0xca, 0x60, 0xc0, 0x29, 0x23, 0xab, 0x0d, 0x53, 0x4e, 0x6f,
  0xd5, 0xdb, 0x37, 0x45, 0xde, 0xfd, 0x8e, 0x2f, 0x03, 0xff, 0x6a, 0x72, 0x6d, 0x6c, 0x5b, 0x51,
  0x8d, 0x1b, 0xaf, 0x92, 0xbb, 0xdd, 0xbc, 0x7f, 0x11, 0xd9, 0x5c, 0x41, 0x1f, 0x10, 0x5a, 0xd8,
  0x0a, 0xc1, 0x31, 0x88, 0xa5, 0xcd, 0x7b, 0xbd, 0x2d, 0x74, 0xd0, 0x12, 0xb8, 0xe5, 0xb4, 0xb0,
  0x89, 0x69, 0x97, 0x4a, 0x0c, 0x96, 0x77, 0x7e, 0x65, 0xb9, 0xf1, 0x09, 0xc5, 0x6e, 0xc6, 0x84,
  0x18, 0xf0, 0x7d, 0xec, 0x3a, 0xdc, 0x4d, 0x20, 0x79, 0xee, 0x5f, 0x3e, 0xd7, 0xcb, 0x39, 0x48];

var CK = [
  0x00070e15, 0x1c232a31, 0x383f464d, 0x545b6269,
  0x70777e85, 0x8c939aa1, 0xa8afb6bd, 0xc4cbd2d9,
  0xe0e7eef5, 0xfc030a11, 0x181f262d, 0x343b4249,
  0x50575e65, 0x6c737a81, 0x888f969d, 0xa4abb2b9,
  0xc0c7ced5, 0xdce3eaf1, 0xf8ff060d, 0x141b2229,
  0x30373e45, 0x4c535a61, 0x686f767d, 0x848b9299,
  0xa0a7aeb5, 0xbcc3cad1, 0xd8dfe6ed, 0xf4fb0209,
  0x10171e25, 0x2c333a41, 0x484f565d, 0x646b7279];

var FK = [0xa3b1bac6, 0x56aa3350, 0x677d9197, 0xb27022dc];

function getHeight4 (data) {//获取高四位
  var height = ((data & 0xf0) >> 4);
  return height;
}

function getLow4 (data) {//获取低四位
  var low = (data & 0x0f);
  return low;
}

//8比特的s盒变换
function sm4Sbox (inch) {
  var i = (inch) & 0xff;
  var retVal = int2Byte(sboxTable[i]);

  return retVal;
}

//算法Lt(.)
function sm4Lt (ka) {
  var bb = (0);
  var c = (0);
  var a = new Array(4);
  var b = new Array(4);
  PUT_ULONG_BE(ka, a, 0);
  b[0] = sm4Sbox(a[0]);
  b[1] = sm4Sbox(a[1]);
  b[2] = sm4Sbox(a[2]);
  b[3] = sm4Sbox(a[3]);
  bb = GET_ULONG_BE(b, 0);
  c = bb ^ ROTL(bb, 2) ^ ROTL(bb, 10) ^ ROTL(bb, 18) ^ ROTL(bb, 24);

  //console.log('ka:' + ka + ',retVal:' + c);
  return (c);
}

//算法F()，即一轮变换
function sm4F (x0, x1, x2, x3, rk) {
  return (x0 ^ sm4Lt(x1 ^ x2 ^ x3 ^ rk));
}


function sm4CalciRK (ka) {
  var bb = 0;
  var rk = 0;
  var a = new Array(4);
  var b = new Array(4);
  PUT_ULONG_BE(ka, a, 0);
  b[0] = sm4Sbox(a[0]);
  b[1] = sm4Sbox(a[1]);
  b[2] = sm4Sbox(a[2]);
  b[3] = sm4Sbox(a[3]);
  bb = GET_ULONG_BE(b, 0);
  rk = bb ^ ROTL(bb, 13) ^ ROTL(bb, 23);
  return rk;
}

//轮密钥生成
function sm4_setkey (SK, key) {
  var MK = new Array(4);
  var k = new Array(36);
  var i = 0;

  //alert("key:" + key);
  // alert("aaa")
  MK[0] = GET_ULONG_BE(key, 0);
  MK[1] = GET_ULONG_BE(key, 4);
  MK[2] = GET_ULONG_BE(key, 8);
  MK[3] = GET_ULONG_BE(key, 12);
  k[0] = MK[0] ^ parseInt(FK[0]);
  k[1] = MK[1] ^ parseInt(FK[1]);
  k[2] = MK[2] ^ parseInt(FK[2]);
  k[3] = MK[3] ^ parseInt(FK[3]);
  for (; i < 32; i++) {
    k[(i + 4)] = (k[i] ^ sm4CalciRK(k[(i + 1)] ^ k[(i + 2)] ^ k[(i + 3)] ^ parseInt(CK[i])));
    SK[i] = k[(i + 4)];
  }
  //alert("SK:" + SK.length + " " + SK[0])
}

//
function sm4_rounds (sk, input, output) {
  i = 0;
  ulbuf = new Array(36);
  ulbuf[0] = GET_ULONG_BE(input, 0);
  ulbuf[1] = GET_ULONG_BE(input, 4);
  ulbuf[2] = GET_ULONG_BE(input, 8);
  ulbuf[3] = GET_ULONG_BE(input, 12);
  while (i < 32) {
    ulbuf[(i + 4)] = sm4F(ulbuf[i], ulbuf[(i + 1)], ulbuf[(i + 2)], ulbuf[(i + 3)], sk[i]);
    i++;
  }
  PUT_ULONG_BE(ulbuf[35], output, 0);
  PUT_ULONG_BE(ulbuf[34], output, 4);
  PUT_ULONG_BE(ulbuf[33], output, 8);
  PUT_ULONG_BE(ulbuf[32], output, 12);
}

function padding (input, mode) {
  if (input == undefined) {
    return null;
  }

  var ret = null;
  if (mode == 1)//填充
  {
    var p = 16 - input.length % 16;
    ret = new Array(input.length + p);
    arrayCopy(input, 0, ret, 0, input.length);
    for (var i = 0; i < p; i++) {
      ret[input.length + i] = p;
    }
  }
  else//去除填充
  {
    var p = input[input.length - 1];
    ret = new Array(input.length - p);
    arrayCopy(input, 0, ret, 0, input.length - p);
  }
  return ret;
}

//生成加密密钥
function sm4_setkey_enc (ctx, key) {

  if (ctx == undefined) {
    Error("ctx is null!");
  }

  if (key == undefined || key.length < 16) {//
    Error("key error!");
  }
  // alert("ctx"+ctx.sk.length);
  ctx.mode = 1;
  sm4_setkey(ctx.sk, key);
}

//生成解密密钥
function sm4_setkey_dec (ctx, key) {
  if (ctx == null) {
    Error("ctx is null!");
  }

  if (key == null || key.length < 16) {//
    Error("key error!");
  }

  var i = 0;
  ctx.mode = 0;
  sm4_setkey(ctx.sk, key);

  //ctx.sk = ctx.sk.reverse();
  for (i = 0; i < 16; i++) {
    SWAP(ctx.sk, i);
  }
}

function sm4_crypt_ecb (ctx, input) {
  // alert("input"+input[0]+input[1]);
  // alert("input-size"+input.length);
  if (isNull(input)) {
    Error("input is null!");
  }
  if ((ctx.isPadding) && (ctx.mode == 1)) {
    input = padding(input, 1);
  }
  // alert("input-size"+input.length)
  var length = input.length;
  var blockLen = 16;
  var loop = Math.ceil(length / blockLen);//注意不能整除会有小数，要取整
  var bous = new Array((loop) * blockLen);

  //for (var i = 0; i < loop; i++) {
  for (var i = loop - 1; i >= 0; i--) {
    var inn = new Array(blockLen);
    var out = new Array(blockLen);
    arrayCopy(input, i * blockLen, inn, 0, blockLen);
    sm4_rounds(ctx.sk, inn, out);
    arrayCopy(out, 0, bous, i * blockLen, blockLen);
  }

  // alert("bous-size"+bous.length);
  if (ctx.isPadding && ctx.mode == 0) {
    bous = padding(bous, 0);
  }
  return bous;
}


function sm4_crypt_cbc (ctx, iv, input) {
  if (isNull(input)) {
    Error("input is null!");
  }
  if ((ctx.isPadding) && (ctx.mode == 1)) {
    input = padding(input, 1);
  }
  // alert("input-size"+input.length)
  var length = input.length;
  var blockLen = 16;
  var loop = Math.ceil(length / blockLen);//注意不能整除会有小数，要取整
  var bous = [];//new Array((loop) * blockLen);
  if (ctx.mode == 1) {
    for (var i = 0; i < loop; i++) {
      //for (var i = loop - 1; i >= 0; i--) {
      var inn = new Array(blockLen);
      var out = new Array(blockLen);
      var inn1 = new Array(blockLen);

      arrayCopy(input, i * blockLen, inn, 0, length > blockLen ? blockLen : length);
      for (j = 0; j < 16; j++) {
        inn1[j] = ((inn[j] ^ iv[j]));
      }
      sm4_rounds(ctx.sk, inn1, out);
      arrayCopy(out, 0, iv, 0, blockLen);
      bous = bous.concat(out);//arrayCopy(out, 0, bous, i * blockLen, blockLen);
    }
  } else {
    var temp = new Array(blockLen);
    for (var i = 0; i < loop; i++) {
      //for (var i = loop - 1; i >= 0; i--) {
      var inn = new Array(blockLen);
      var out = new Array(blockLen);
      var out1 = new Array(blockLen);

      arrayCopy(input, i * blockLen, inn, 0, length > blockLen ? blockLen : length);
      arrayCopy(inn, 0, temp, 0, blockLen);
      sm4_rounds(ctx.sk, inn, out);
      for (j = 0; j < 16; j++) {
        out1[j] = ((out[j] ^ iv[j]));
      }
      arrayCopy(temp, 0, iv, 0, blockLen);
      bous = bous.concat(out1);//arrayCopy(out1, 0, bous, i * blockLen, blockLen);
    }
  }
  // alert("bous-size"+bous.length);
  if (ctx.isPadding && ctx.mode == 0) {
    bous = padding(bous, 0);
  }
  return bous;
}
//sm4utils.js
//===========================================================================================================
function sm4utils () {
  this.encrypt_ECB = encryptData_ECB;
  this.decrypt_ECB = decryptData_ECB;
  this.encrypt_CBC = encryptData_CBC;
  this.decrypt_CBC = decryptData_CBC;

  // this.hexString = false;
  function encryptData_ECB (plainText, seckey) {
    var ctx = new context();
    ctx.isPadding = true;
    ctx.mode = 1;

    var keyBytes;
    try {
      if (isNull(seckey)) {
        console.log("sm4 key is error!");
        return null;
      }
      keyBytes = Hex.utf8StrToBytes(seckey);

    } catch (e) {
      Error(e.message);
    }
    sm4_setkey_enc(ctx, keyBytes);
    var encrypted = sm4_crypt_ecb(ctx, Hex.utf8StrToBytes(plainText));
    var cipherText = Hex.encode(encrypted, 0, encrypted.length);
    // alert(cipherText);
    return cipherText;
  }

  function decryptData_ECB (cipherText, seckey) {
    try {
      var ctx = new context();
      ctx.isPadding = true;
      ctx.mode = 0;
      //alert('cipherText:' + cipherText.length);
      var keyBytes;
      try {
        if (isNull(seckey)) {
          console.log("sm4 key is error!");
          return null;
        }
        keyBytes = Hex.utf8StrToBytes(seckey);

      } catch (e) {
        Error(e.message);
      }
      var data = Hex.decode(cipherText);
      sm4_setkey_dec(ctx, keyBytes);
      var decrypted = sm4_crypt_ecb(ctx, data);
      return Hex.bytesToUtf8Str(decrypted);
    } catch (e) {
      Error(e.message);
      return null;
    }
  }

  function encryptData_CBC (plainText, seckey, iv) {
    var ctx = new context();
    ctx.isPadding = true;
    ctx.mode = 1;

    var keyBytes
    var ivBytes;
    try {
      if (isNull(seckey)) {
        console.log("sm4 key is error!");
        return null;
      }
      if (isNull(iv)) {
        console.log("sm4 iv is error!");
        return null;
      }
      keyBytes = Hex.utf8StrToBytes(seckey);
      ivBytes = Hex.utf8StrToBytes(iv);

    } catch (e) {
      Error(e.message);
    }
    sm4_setkey_enc(ctx, keyBytes);
    var encrypted = sm4_crypt_cbc(ctx, ivBytes, Hex.utf8StrToBytes(plainText));
    var cipherText = Hex.encode(encrypted, 0, encrypted.length);
    // alert(cipherText);
    return cipherText;
  }

  function decryptData_CBC (cipherText, seckey, iv) {
    try {
      var ctx = new context();
      ctx.isPadding = true;
      ctx.mode = 0;
      //alert('cipherText:' + cipherText.length);
      var keyBytes
      var ivBytes;
      try {
        if (isNull(seckey)) {
          console.log("sm4 key is error!");
          return null;
        }
        if (isNull(iv)) {
          console.log("sm4 iv is error!");
          return null;
        }
        keyBytes = Hex.utf8StrToBytes(seckey);
        ivBytes = Hex.utf8StrToBytes(iv);

      } catch (e) {
        Error(e.message);
      }
      var data = Hex.decode(cipherText);
      sm4_setkey_dec(ctx, keyBytes);
      var decrypted = sm4_crypt_cbc(ctx, ivBytes, data);
      return Hex.bytesToUtf8Str(decrypted);
    } catch (e) {
      Error(e.message);
      return null;
    }
  }
}