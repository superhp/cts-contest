export const fakeDuelInfo = {
    players: [{
        isLoggedIn: true,
        email: 'a@a.com',
        name: 'Petras',
        totalBalance: 0,
        totalWins: 0,
        totalLooses: 0
    }, {
        isLoggedIn: true,
        email: 'a@a.com',
        name: 'Antanas',
        totalBalance: 0,
        totalWins: 0,
        totalLooses: 0
    }],
    startTime: new Date('1995-12-17T03:24:00'),
    task: {
        id: 1234,
        description: `<p><span style="font-weight: 400;">There once was a wise servant who saved the life of a prince. The king promised to pay whatever the servant could dream up. Knowing that the king loved chess, the servant told the king he would like to have grains of wheat. One grain on the first square of the chessboard, two grains on the next, four on the third, and so on. Given the coordinates of a square on the chessboard, count how many grains of wheat the king has to put on that specific square. The numbering of the squares is given in the image below:</span></p>
        <p><span style="font-weight: 400;"><img style="width: 500px; height: 500px; display: block; margin-left: auto; margin-right: auto;" src="https://cts-contest-cms.azurewebsites.net/media/1101/chess_board.jpg?width=500&amp;height=500" alt="" data-udi="umb://media/4f1dd52b911d4be68341296ef7e29125" /></span></p><p><strong>Input:</strong></p><p><span style="font-weight: 400;">The input consists of a single line with the coordinates of the square. The coordinates are given in algebraic notation. The letters in this notation are guaranteed to be lowercase.</span></p><p><strong>Output:</strong></p><p><span style="font-weight: 400;">The output should contain a single positive integer - the number of grains of wheat on that square.</span></p><p><strong>Example:</strong></p><table><thead><tr><th>Input</th><th>Output</th></tr></thead><tbody><tr><td>a1</td><td>1</td></tr><tr><td>b2</td><td>512</td></tr></tbody></table>`,
        isSolved: false,
        name: 'Great task',
        value: 10
    }
};