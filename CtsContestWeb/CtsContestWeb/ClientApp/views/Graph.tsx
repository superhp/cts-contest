export enum NodeType { Q , A }

class GraphNode {
    id: number;
    text: string;
    edges: Edge[];
    type: NodeType;

    constructor(id: number, text: string, type: NodeType) {
        this.id = id;
        this.text = text;
        this.type = type;
        this.edges = [];
    }
}

class Edge {
    id: number;
    text: string;

    constructor(id: number, text: string) {
        this.id = id;
        this.text = text;
    }
}

class Graph {
    nodes: GraphNode[];
    curr: GraphNode;
    constructor() {
        this.nodes = [];
    }

    addQuestion(idx: number, question: string) {
        var node = new GraphNode(idx, question, NodeType.Q);
        this.nodes.push(node);
    }

    addAnswer(idx: number, answer: string) {
        var node = new GraphNode(idx, answer, NodeType.A);
        this.nodes.push(node);
    }

    addEdge(from: number, to: number, text: string) {
        var fromNode = this.nodes.filter(x => x.id === from)[0];
        fromNode.edges.push({
            id: to,
            text: text
        });
    }

    setCurr(id: number) {
        this.curr = this.nodes.filter(x => x.id === id)[0];
    }

    answer(ansText: string) {
        var idTo = this.curr.edges.filter(x => x.text == ansText)[0].id;
        this.setCurr(idTo);
    }
}

const g = new Graph();
g.addQuestion(0, 'Do you like new career challenges & opportunities?');
g.addQuestion(1, 'Does <div style="\r\n    text-transform:  none;\r\n    font-family:  serif;\r\n    color: #00b140;\r\n">\r\n    <span>if (careerDays) { </span><br>\r\n    <span style="\r\n    margin-left: 15px;\r\n">    console.log("meet Cognizant colleagues");</span><br>\r\n    <span>} else {</span><br>\r\n    <span style="\r\n    margin-left: 15px;\r\n">console.log("back to lectures");</span><br>\r\n    <span>}</span>\r\n</div>mean anything to you?');
g.addQuestion(2, 'Do you feel experienced enough to dive into international IT ocean?');
g.addQuestion(3, 'If you were a musician, what would you prefer?');
g.addAnswer(4, 'Don\'t worry, You can grow your knowledge together with us. Find Marija, tell her the code <span style="color: #00b140">"1001"</span> and You will get a gift.');
g.addAnswer(5, '<span style="color: #00b140">Congratulations!</span> You are meant to be a <span style="color: #00b140">front end developer</span>. Find Marija, tell her the code <span style="color: #00b140">"FR10"</span> and You will get a gift.');
g.addAnswer(6, '<span style="color: #00b140">Congratulations!</span> You are meant to be a <span style="color: #00b140">back end developer</span>. Find Marija, tell her the code <span style="color: #00b140">"BC11"</span> and You will get a gift.');
g.addAnswer(7, '<span style="color: #00b140">Congratulations!</span> You are meant to be a <span style="color: #00b140">test analyst</span>. Find Marija, tell her the code <span style="color: #00b140">"TS01"</span> and You will get a gift.');
g.addAnswer(8, '<span style="color: #00b140">Try again :)</span>');
g.addQuestion(9, 'Can You give a positive answer to this question: <span style="color: #00b140">"Snakker du dansk, norsh eller svensk"?</span>');
g.addQuestion(10, 'Do You feel strong enough to use Danish, Norwegian or Swedish language at work?');
g.addQuestion(11, 'When you are planning a trip with friends, what role do you prefer?');
g.addAnswer(12, '<span style="color: #00b140">Congratulations!</span> You are meant to work in <span style="color: #00b140">finance</span> field. Find Ieva, tell her the code <span style="color: #00b140">"FIN1"</span> and You will get a gift.');
g.addAnswer(13, '<span style="color: #00b140">Congratulations!</span> You are meant to work with <span style="color: #00b140">business process managment</span>. Find Ieva, tell her the code <span style="color: #00b140">"PEX1"</span> and You will get a gift.');
g.addQuestion(14, 'Would You like to become a guru in one of the Scandinavian languages?');
g.addAnswer(15, '<span style="color: #00b140">Congratulations!</span> You could fulfill this dream by joining our language courses and becoming a <span style="color: #00b140">Jr. Finance Specialist</span>. Find Ieva, tell her the code <span style="color: #00b140">"April16"</span> and You will get a gift.');
g.addQuestion(16, 'When you are planning a trip with friends, what role do you prefer?');
g.addAnswer(17, '<span style="color: #00b140">Congratulations!</span> You are meant to work with <span style="color: #00b140">analytics</span>. Find Ieva, tell her the code <span style="color: #00b140">"BA02"</span> and You will get a gift.');
g.addAnswer(18, '<span style="color: #00b140">Congratulations!</span> You are meant to work with <span style="color: #00b140">business process management</span>. Find Marija, tell her the code <span style="color: #00b140">"PM01"</span> and You will get a gift.');

g.addEdge(0, 1, "YES");
g.addEdge(0, 8, "NO");
g.addEdge(1, 2, "YES");
g.addEdge(1, 9, "NO");
g.addEdge(2, 3, "YES");
g.addEdge(2, 4, "NO");
g.addEdge(3, 5, "FRONT STAGE");
g.addEdge(3, 6, "BACK STAGE");
g.addEdge(3, 7, "SOUND QUALITY TESTER");
g.addEdge(9, 10, "YES");
g.addEdge(10, 11, "YES");
g.addEdge(11, 12, "BUDGET MASTER");
g.addEdge(11, 13, "ACTIVITIES GURU");
g.addEdge(9, 14, "NO");
g.addEdge(10, 14, "NO");
g.addEdge(14, 15, "YES");
g.addEdge(14, 16, "NO");
g.addEdge(16, 18, "ACTIVITIES GURU");
g.addEdge(16, 17, "NAVIGATION OWNER");

g.setCurr(0);
export default g;