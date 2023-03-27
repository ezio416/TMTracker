// c 2023-03-25
// m 2023-03-27

namespace TMT.Models;

public class StyledString {
    public List<StyledChar> chars { get; set; }
    public string           raw   { get; set; }
    public string           text  { get; set; }

    public StyledString(string rawInput) {
        raw = rawInput;

        chars = new();
        bool bold = false;
        List<char> code = new();
        string color_3 = "FFF";
        int colorCountdown = 0;
        bool flag = false;
        bool isCharVal = true;
        bool italic = false;
        bool narrow = false;
        bool shadow = false;
        List<char> Text = new();
        bool wide = false;

        foreach (char ch in raw) {
            bool isCharHex = Various.IsCharHex(ch);
            char CH = char.ToUpper(ch);
            isCharVal = true;
            if (colorCountdown > 0) {
                if (isCharHex) {
                    colorCountdown -= 1;
                    flag = true;
                } else {
                    for (int i = 0; i <= 3 - code.Count; i++)
                        code.Add('0');
                    colorCountdown = 0;
                    flag = false;
                }
            }
            if (code.Count == 3) {
                color_3 = new string(code.ToArray());
                code.Clear();
            }
            if (ch == '$') {
                flag = true;
                continue;
            }
            if (flag) {
                flag = false;
                isCharVal = false;
                switch (CH) {
                    case 'G':  // reset color
                        color_3 = "FFF";
                        break;
                    case 'I':
                        italic = true;
                        break;
                    case 'L':  // clickable link
                        continue;
                    case 'N':
                        narrow = true;
                        break;
                    case 'O':
                        bold = true;
                        break;
                    case 'S':
                        shadow = true;
                        break;
                    case 'T': // uppercase
                        continue;
                    case 'W':
                        wide = true;
                        break;
                    case 'Z':  // reset styling
                        bold = false;
                        italic = false;
                        narrow = false;
                        shadow = false;
                        wide = false;
                        break;
                    case '$':  // escaped '$'
                        isCharVal = true;
                        break;
                    default:
                        if (isCharHex) {
                            if (code.Count == 0)
                                colorCountdown = 2;
                            code.Add(CH);
                        }
                        break;
                }
            }
            if (isCharVal) {
                chars.Add(new StyledChar {
                    bold = bold,
                    color_3 = color_3,
                    italic = italic,
                    narrow = narrow,
                    shadow = shadow,
                    value = ch,
                    wide = wide
                });
                Text.Add(ch);
            }
        }

        text = new string(Text.ToArray());
    }
}