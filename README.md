**Project Description**
Experimental tool sets in mud parsing and area definition

This is a simple code-base that will experiment in using object oriented designs to handle parsing and room definition.

The ParserTest branch is a proof of concept project for a relatively simple parser that supports a number of natural language constructs such as;

	* Get the Sword
	* Get the blue sword (when there are multiple items of the same type to check)
	* Get the blue sword and put it in the chest (uses AND as a connector and remembers the results of GET to use as IT)

The general design is that all parable items in the room provide an interface that describes the element in generic terms (basic name, adjectives, specific name, etc..) and the parser then uses that information to feed a series of registered Verb Actions.

When registered, verbs specify what types of arguments they are looking for and the parser matches them up based on the user's text and the context of the room the user is in. The parser uses a language interface that is attached to the player to look at the words coming in and determine what words are fillers or connectors and attempts to build up a validated argument list for the verbs to act on. This allows the verb action code to not have to do any of it's own parsing and can rely on having a reference to in game objects to work on.

The proof of concept isn't perfect, it has a number of flaws and isn't generic enough, but it shows that the general idea works. The master branch is being developed as a more generic implementation that solves some of the flaws in the test, such as using AND to specific multiple subjects for a verb (Get the Red and Blue Swords)
