Nonin - Norwegian National Identification Numbers
=================================================

A .NET library for validating all kinds of Norwegian identify numbers: 
- The normal citizen number (fødselsnummer)
- D-numbers, H-numbers and FH-numbers

**Note:** The included test data is all randomly generated (using fnr.js).

## Example use


```csharp
using Nonin;
// .....

// Randomly generated sample numbers. If they are in use, I have no idea who they belong to.
var sampleNormal = "29100207042";
var sampleD = "43022014103";

//Simple validity check
Nin.IsValid(sampleNormal); // true
Nin.IsValid(sampleD); // true

Nin.IsValid(sampleD, NinKind.BirthNumber); // false
Nin.IsValid(sampleD, NinKind.BirthNumber, NinKind.DNumber); // true

// Nins can be constructed using new, or using TryParse.
// the constructor will throw if the Nin is invalid.

Nin info = new Nin(sampleD);

if(Nin.TryParse(sampleNormal, out info)) {
	Console.WriteLine(info.Kind.ToString()); //BirthNumber, DNumber, HNumber or FHNumber
	Console.WriteLine(info.Gender.ToString()); // Male, Female or Unspecified (for FH numbers)
	Console.WriteLine(info.DateOfBirth.ToString()); // Date of birth, or null for FH numbers. Note that the Dob in H numbers might be fictional.
}

// You can also get more detailed information about validation errors
// For example:
Nin.Validate("123456"); //Returns NinValidation.TooShort
Nin.Validate("00000000000"); //Returns NinValidation.InvalidControlDigit
Nin.Validate("aaaaaaaaaaa"); //Returns NinValidation.InvalidCharacter

// Nins are immutable, and can be sorted/compared.
// This also means they can be used for keys in dictionaries, or added to sets.
// The sort order is simple lexographic.
Debug.Assert(new Nin(sampleD) == new Nin(sampleD));
Debug.Assert(new Nin(sampleD) != new Nin(sampleNormal));
```
