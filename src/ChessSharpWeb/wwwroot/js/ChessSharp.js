function drawForWhite() {
    var boardHtml = '';
    boardHtml += '<div class="rank">' +
        '<span id="a8" class="square light-square"></span>' +
        '<span id="b8" class="square dark-square"></span>' +
        '<span id="c8" class="square light-square"></span>' +
        '<span id="d8" class="square dark-square"></span>' +
        '<span id="e8" class="square light-square"></span>' +
        '<span id="f8" class="square dark-square"></span>' +
        '<span id="g8" class="square light-square"></span>' +
        '<span id="h8" class="square dark-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="a7" class="square dark-square"></span>' +
        '<span id="b7" class="square light-square"></span>' +
        '<span id="c7" class="square dark-square"></span>' +
        '<span id="d7" class="square light-square"></span>' +
        '<span id="e7" class="square dark-square"></span>' +
        '<span id="f7" class="square light-square"></span>' +
        '<span id="g7" class="square dark-square"></span>' +
        '<span id="h7" class="square light-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="a6" class="square light-square"></span>' +
        '<span id="b6" class="square dark-square"></span>' +
        '<span id="c6" class="square light-square"></span>' +
        '<span id="d6" class="square dark-square"></span>' +
        '<span id="e6" class="square light-square"></span>' +
        '<span id="f6" class="square dark-square"></span>' +
        '<span id="g6" class="square light-square"></span>' +
        '<span id="h6" class="square dark-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="a5" class="square dark-square"></span>' +
        '<span id="b5" class="square light-square"></span>' +
        '<span id="c5" class="square dark-square"></span>' +
        '<span id="d5" class="square light-square"></span>' +
        '<span id="e5" class="square dark-square"></span>' +
        '<span id="f5" class="square light-square"></span>' +
        '<span id="g5" class="square dark-square"></span>' +
        '<span id="h5" class="square light-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="a4" class="square light-square"></span>' +
        '<span id="b4" class="square dark-square"></span>' +
        '<span id="c4" class="square light-square"></span>' +
        '<span id="d4" class="square dark-square"></span>' +
        '<span id="e4" class="square light-square"></span>' +
        '<span id="f4" class="square dark-square"></span>' +
        '<span id="g4" class="square light-square"></span>' +
        '<span id="h4" class="square dark-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="a3" class="square dark-square"></span>' +
        '<span id="b3" class="square light-square"></span>' +
        '<span id="c3" class="square dark-square"></span>' +
        '<span id="d3" class="square light-square"></span>' +
        '<span id="e3" class="square dark-square"></span>' +
        '<span id="f3" class="square light-square"></span>' +
        '<span id="g3" class="square dark-square"></span>' +
        '<span id="h3" class="square light-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="a2" class="square light-square"></span>' +
        '<span id="b2" class="square dark-square"></span>' +
        '<span id="c2" class="square light-square"></span>' +
        '<span id="d2" class="square dark-square"></span>' +
        '<span id="e2" class="square light-square"></span>' +
        '<span id="f2" class="square dark-square"></span>' +
        '<span id="g2" class="square light-square"></span>' +
        '<span id="h2" class="square dark-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="a1" class="square dark-square"></span>' +
        '<span id="b1" class="square light-square"></span>' +
        '<span id="c1" class="square dark-square"></span>' +
        '<span id="d1" class="square light-square"></span>' +
        '<span id="e1" class="square dark-square"></span>' +
        '<span id="f1" class="square light-square"></span>' +
        '<span id="g1" class="square dark-square"></span>' +
        '<span id="h1" class="square light-square"></span>' +
        '</div>';
    document.getElementById('GameBoard').innerHTML = boardHtml;

}
function drawForBlack() {
    var boardHtml = '';
    boardHtml += '<div class="rank">' +
        '<span id="h1" class="square light-square"></span>' +
        '<span id="g1" class="square dark-square"></span>' +
        '<span id="f1" class="square light-square"></span>' +
        '<span id="e1" class="square dark-square"></span>' +
        '<span id="d1" class="square light-square"></span>' +
        '<span id="c1" class="square dark-square"></span>' +
        '<span id="b1" class="square light-square"></span>' +
        '<span id="a1" class="square dark-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="h2" class="square dark-square"></span>' +
        '<span id="g2" class="square light-square"></span>' +
        '<span id="f2" class="square dark-square"></span>' +
        '<span id="e2" class="square light-square"></span>' +
        '<span id="d2" class="square dark-square"></span>' +
        '<span id="c2" class="square light-square"></span>' +
        '<span id="b2" class="square dark-square"></span>' +
        '<span id="a2" class="square light-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="h3" class="square light-square"></span>' +
        '<span id="g3" class="square dark-square"></span>' +
        '<span id="f3" class="square light-square"></span>' +
        '<span id="e3" class="square dark-square"></span>' +
        '<span id="d3" class="square light-square"></span>' +
        '<span id="c3" class="square dark-square"></span>' +
        '<span id="b3" class="square light-square"></span>' +
        '<span id="a3" class="square dark-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="h4" class="square dark-square"></span>' +
        '<span id="g4" class="square light-square"></span>' +
        '<span id="f4" class="square dark-square"></span>' +
        '<span id="e4" class="square light-square"></span>' +
        '<span id="d4" class="square dark-square"></span>' +
        '<span id="c4" class="square light-square"></span>' +
        '<span id="b4" class="square dark-square"></span>' +
        '<span id="a4" class="square light-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="h5" class="square light-square"></span>' +
        '<span id="g5" class="square dark-square"></span>' +
        '<span id="f5" class="square light-square"></span>' +
        '<span id="e5" class="square dark-square"></span>' +
        '<span id="d5" class="square light-square"></span>' +
        '<span id="c5" class="square dark-square"></span>' +
        '<span id="b5" class="square light-square"></span>' +
        '<span id="a5" class="square dark-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="h6" class="square dark-square"></span>' +
        '<span id="g6" class="square light-square"></span>' +
        '<span id="f6" class="square dark-square"></span>' +
        '<span id="e6" class="square light-square"></span>' +
        '<span id="d6" class="square dark-square"></span>' +
        '<span id="c6" class="square light-square"></span>' +
        '<span id="b6" class="square dark-square"></span>' +
        '<span id="a6" class="square light-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="h7" class="square light-square"></span>' +
        '<span id="g7" class="square dark-square"></span>' +
        '<span id="f7" class="square light-square"></span>' +
        '<span id="e7" class="square dark-square"></span>' +
        '<span id="d7" class="square light-square"></span>' +
        '<span id="c7" class="square dark-square"></span>' +
        '<span id="b7" class="square light-square"></span>' +
        '<span id="a7" class="square dark-square"></span>' +
        '</div>';
    boardHtml += '<div class="rank">' +
        '<span id="a8" class="square dark-square"></span>' +
        '<span id="b8" class="square light-square"></span>' +
        '<span id="c8" class="square dark-square"></span>' +
        '<span id="d8" class="square light-square"></span>' +
        '<span id="e8" class="square dark-square"></span>' +
        '<span id="f8" class="square light-square"></span>' +
        '<span id="g8" class="square dark-square"></span>' +
        '<span id="h8" class="square light-square"></span>' +
        '</div>';
    document.getElementById('GameBoard').innerHTML = boardHtml;

}

function getPieceOwnerFromObject(obj) {
    if (obj == null) return '';
    var str = JSON.stringify(obj);

    var owner = (str.includes('0') ? 'White' : 'Black');
    var piece = '';
    if (str.includes('Pawn')) piece = 'Pawn';
    if (str.includes('Knight')) piece = 'Knight';
    if (str.includes('Bishop')) piece = 'Bishop';
    if (str.includes('Rook')) piece = 'Rook';
    if (str.includes('Queen')) piece = 'Queen';
    if (str.includes('King')) piece = 'King';
    return owner + piece;
}

function getSquareName(rank, file) {
    return String.fromCharCode(97 + file) + String.fromCharCode(49 + rank);
}

function showGame(pieces) {
    var x = JSON.parse(pieces);
    for (var i = 0; i < 8; i++) {
        for (var j = 0; j < 8; j++) {
            var piece = getPieceOwnerFromObject(x.Board.$values[i][j]);
            document.getElementById(getSquareName(i, j)).className += ' ' + piece;
        }
    }

}
