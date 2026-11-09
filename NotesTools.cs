using System.ComponentModel;
using ModelContextProtocol.Server;

[McpServerToolType]
public class NotesTools
{
    static string notesFolder = @"C:\Users\bzdav\OneDrive\Projects\claude-code\2nd-Brain\ask-MCP_DB\notes";

    [McpServerTool(Name = "search_notes")]
    [Description("Searches the team's runbooks and past incident notes. Use this before querying the database.")]
    public static string SearchNotes(string query)
    {
        string[] words = query.ToLower().Split(' ');
        string bestFile = "";
        string bestText = "";
        int bestScore = 0;

        foreach (string file in Directory.GetFiles(notesFolder, "*.md"))
        {
            string text = File.ReadAllText(file);
            string lowerText = text.ToLower();

            int score = 0;
            foreach (string word in words)
            {
                if (word.Length > 3 && lowerText.Contains(word))
                {
                    score++;
                }
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestFile = Path.GetFileName(file);
                bestText = text;
            }
        }

        if (bestScore == 0)
        {
            return "No matching notes.";
        }
        return "From " + bestFile + ":\n" + bestText;
    }
}
