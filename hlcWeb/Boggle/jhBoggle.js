var jhBoggle = (function() {
    var _version = "1.0";
    var _author = "Jeff Holman";
    var ox_appid = "41d2606f";
    var ox_appkey = "ac3ee153ab384196c4bc7045b180d7e5";
    var _board = [];

    var randomNumber = function(max) {
        return Math.floor(Math.random() * max + 1);
    }

    /*
     * generateBoard  -  Generate a random Boggle board
     * 
     * The standard Super Big Boggle cubes (6x6) are used.  Simulate shaking the board and filling the 6x6 slots as follows:
     *  1: Select a random cube from the cubes list
     *  2: Select a random letter from the selected cube to go on the board
     *  3: Remove the selected cube from the list
     *  4: Repeat until all cubes are used.
     */
    var generateBoard = function() {
        console.info("--Generating random board...");

        // Standard Super Big Boggle cube faces (6x6 a=An e=Er h=He i=In q=Qu t=Th)
        var cubes = [
            "AAAFRS", "AAEEEE", "AAEEOO", "AAFIRS", "ABDEIO", "ADENNN", "AEEEEM", "AEEGMU", "AEGMNN", "AEILMN", "AEINOU",
            "AFIRSY", "aehiqt ", "BBJKXZ", "CCENST", "CDDLNN", "CEIITT", "CEIPST", "CFGNUY", "DDHNOT", "DHHLOR", "DHHNOW",
            "DHLNOR", "EHILRS", "EIILST", "EILPST", "EIO###", "EMTTTO", "ENSSSU", "GORRVW", "HIRSTV", "HOPRST", "IPRSYY",
            "JKqWXZ", "NOOTUW", "OOOTTU"
        ];

        var newBoard = new Array();
        //debugger;
        for (var i = 0; i < 6; i++) {

            // Create a new, empty row for the board as an array to hold the cubes on that row
            newBoard[i] = new Array();

            // For each cube on this row...
            for (var j = 0; j < 6; j++) {
                
                var cubeIndex    = randomNumber(cubes.length) - 1;       // pick random cube index (-1 to adjst for zero-based array)
                var selectedCube = cubes[cubeIndex];                     // get the faces of the cube
                var sideIndex    = randomNumber(6) - 1;                  // pick a random side of the selected cube
                var letter       = selectedCube[sideIndex];              // and get the letter off the selected cube
                switch (letter) {                                        // handle special two-letter faces
                    case "a":
                        letter = "An";
                        break;
                    case "e":
                        letter = "Er";
                        break;
                    case "h":
                        letter = "He";
                        break;
                    case "i":
                        letter = "In";
                        break;
                    case "q":
                        letter = "Qu";
                        break;
                    case "t":
                        letter = "Th";
                        break;
                    default:
                }
                newBoard[i][j]   = letter;                               // add it to the board
                cubes.splice(cubeIndex, 1);                              // remove the selected cube from the available cubes
            }
        }
        _board = newBoard;        
        console.info(_board);
    }

    return {
        version: _version,
        author: _author,

        initialize: function () {
            console.info("jhBoggle - version " + _version);
            console.info("--Initializing...");
            generateBoard();
            return _board;
        },

        shakeBoard: function () {
            generateBoard();
            return _board;
        },

        rollDie: randomNumber
    };
})();
