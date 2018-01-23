using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TextStatic : MonoBehaviour 
{

    #region public data

    #endregion

    #region private data

    private List < GameObject > _alphabet = new List<GameObject>(); // all the letters of the alphabet
    private List < GameObject > _letters = new List<GameObject>(); // letters in the given phrase

    private GameObject newLetter;
    private int size;
    private Vector3 origPos;
    private Quaternion origRot;
    private Vector3 origScale;
    private float tracking = .5f;

    [SerializeField] string firstPhrase;
    private enum FontSize { Small, Medium, Large }
    [SerializeField] FontSize fontSize;

    #endregion

    #region private functions

    private void MakeTextGO ( string text )
    {
        Clear ( );

        //center text

        // Default is large 
        size = text.Length;

        if ( fontSize == FontSize.Small )
        {
            size /= 3;
        }
        else if ( fontSize == FontSize.Medium )
        {
            size = size / 3 * 2;
        }
        
        float length = size * tracking;
        transform.position += transform.right * length / 2 * transform.localScale.x;

        for ( int i = 0; i < text.Length; i++ )
        {
            int count = 1;
            if ( text [ i ] == ' ' )
            {
                count *= 2; // make a space 
                continue; // jump to the next letter 
            }

            for ( int j = 0; j < _alphabet.Count; j++ )
            {
                if ( text [ i ].ToString ( ) == _alphabet [ j ].name )
                {
                    GameObject newLetter = Instantiate (_alphabet [j]);
                    newLetter.transform.SetParent ( transform );
                    newLetter.transform.localPosition = new Vector3 ( -tracking * count * i, 0f, 0f );
                    newLetter.transform.localRotation = Quaternion.identity;
                    _letters.Add ( newLetter );
                    count = 1;
                }
            }
        }
    }

    private void SwapCharGO ( char c, int item )
    {
        // GameObject newLetter = new GameObject();

        // Assuming no match will yield an empty space
        for ( int j = 0; j < _alphabet.Count; j++ )
        {
            if ( c.ToString ( ) == _alphabet [ j ].name )
            {
                newLetter = Instantiate ( _alphabet [ j ] );
            }
        }

        newLetter.transform.position = _letters [ item ].transform.position;
        newLetter.transform.SetParent ( transform );

        GameObject.Destroy ( _letters [ item ] );
        _letters [ item ] = newLetter;
    }

    private void Clear ( )
    {
        foreach ( Transform child in transform )
        {
            GameObject.Destroy ( child.gameObject );
        }

        _letters.Clear ( );

        transform.position = origPos;
        transform.rotation = origRot;
        transform.localScale = origScale;
    }

    #endregion

    #region inherited functions

    private void Start ( )
    {
        newLetter = new GameObject ( );

        origPos = transform.position;
        origRot = transform.rotation;
        origScale = transform.localScale;

        tracking /= transform.localScale.x;

        // Default is small
        string levelPath = "Models/Letters/Small";
        tracking = .1f;

        if(fontSize == FontSize.Medium)
        {
            levelPath = "Models/Letters/Medium";
            tracking = .5f;
        }
        else if(fontSize == FontSize.Large )
        {
            levelPath = "Models/Letters/Large";
            tracking = 2.5f;
        }

        Object[] alphabet = Resources.LoadAll(levelPath, typeof(GameObject));

        if ( alphabet == null || alphabet.Length == 0 )
        {
            //print ("no files found");
        }

        foreach ( GameObject letter in alphabet )
        {
            GameObject l = (GameObject)letter;
            l.layer = 20;
            _alphabet.Add ( l );
        }

        MakeTextGO ( firstPhrase );
    }

    #endregion
}
