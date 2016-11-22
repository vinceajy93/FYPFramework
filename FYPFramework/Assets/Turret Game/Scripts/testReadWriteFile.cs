using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class testReadWriteFile : MonoBehaviour
{

	//not using, but keep for reference of next batch
	[SerializeField]
	private TextAsset P1CostTextFile, P2CostTextFile;
	private string wholeString_P1, wholeString_P2;
	private List<string> eachline_P1, eachline_P2;

	private Text debugtext, checktext;

	private StreamWriter writer; //writer that writes to the file
	private FileInfo fileinfo;
	// Use this for initialization
	void Start ()
	{
		fileinfo = new FileInfo(Application.persistentDataPath + "//Cost_P1.txt");
		//for debugging
		debugtext = GameObject.Find("debugtext").GetComponent<Text>();
		checktext = GameObject.Find ("checktext").GetComponent<Text> ();
		debugtext.text = "initilaizing";
		ReadfromTextfile ();

		Debug.Log (fileinfo);

		//debugging check
//		for(int i = 1; i < eachline_P1.Count; i++){
//			Debug.Log (eachline_P1[i-1]);
//		}

		//WriteLineToTextfile ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void ReadfromTextfile(){
		wholeString_P1 = P1CostTextFile.text;
		eachline_P1 = new List<string> ();

		eachline_P1.AddRange (wholeString_P1.Split (':'));

	}


	public void WriteLineToTextfile(){
		string NewString = "";



		for(int i = 0; i < eachline_P1.Count; i++){
			
			int tempInt = 0;

			if (eachline_P1 [i] == "turret_1_P1") {
				
				tempInt = int.Parse (eachline_P1 [i + 1]);

				tempInt += 1;
				eachline_P1[i + 1] = tempInt.ToString();
				debugtext.text = tempInt.ToString();
			}

			
			NewString += eachline_P1 [i]+ ":\n";
		
		}

		//write the new file back to the textfile (P1)
		//writer = new StreamWriter (Path);
		System.IO.File.WriteAllText(fileinfo.ToString(), NewString);
		//P1CostTextFile.text = NewString;
		Debug.Log(fileinfo);
		//debugtext.text = ("File Saved: " + fileinfo);
	}

	public void checkStatsInText(){

		ReadfromTextfile ();
		checktext.text = (eachline_P1[1]);
	}
}
	