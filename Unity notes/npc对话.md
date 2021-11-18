    List<string> textList = new List<string>();
    
    // Start is called before the first frame update
    void Awake()
    {
        GetTextFromFile(textFile);
        //index = 0;
    }
    
    private void OnEnable()//Onenable在Start前调用
    {
        textLabel.text = textList[index];
        index++;
    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            textLabel.text = textList[index];
            index++;
        }
    }
    void GetTextFromFile(TextAsset file)
    {
    
        if(Input.GetKeyDown(KeyCode.R)&&index==textList.Count)
        {
            textList.Clear();
            index = 0;
            return; 
        }
    
        var lineData = file.text.Split('\n');//按行切割，变成一个数组
        foreach(var line in lineData)
        {
            textList.Add(line);
        }
    
    }