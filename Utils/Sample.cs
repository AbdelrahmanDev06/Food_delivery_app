namespace FastFoodPOS.Utils;

internal static class Sample
{
    public static void Fill(DataGridView g, string[] headers, string[][] rows, params int[] rightAligned)
    {
        UiHelper.StyleGrid(g);
        g.AutoGenerateColumns = false;
        for (int i = 0; i < headers.Length; i++)
        {
            int index = g.Columns.Add("col" + i, headers[i]);
            if (rightAligned.Contains(i))
            {
                g.Columns[index].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                g.Columns[index].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
        foreach (var row in rows) g.Rows.Add(row);
    }
}
