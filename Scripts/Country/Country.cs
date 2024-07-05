using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Country
{
    public string type;
}

[Serializable]
public class CountryList
{
    public Country[] countries;
}