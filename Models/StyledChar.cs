// c 2023-03-25
// m 2023-03-27

namespace TMT.Models;

public class StyledChar {
    public bool   bold    { get; set; }
    public string color_3 { get; set; }  // RGB
    public string color_6 { get {        // #RRGGBB
        List<char> result = new() { '#' };
            foreach (char ch in color_3) {
                result.Add(ch);
                result.Add(ch);
            }
        return new string(result.ToArray());
    } }
    public bool   italic  { get; set; }
    public bool   narrow  { get; set; }
    public bool   shadow  { get; set; }
    public char   value   { get; set; }
    public bool   wide    { get; set; }
}