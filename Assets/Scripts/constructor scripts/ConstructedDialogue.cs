using System.Collections.Generic;

[System.Serializable]
public class ConstructedDialogue
{
    public string question;                
    public string location;
    public List<ConstructedOption> options = new List<ConstructedOption>();
}
