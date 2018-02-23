enum NodeType { Q , A }

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
}

const g = new Graph();
g.addQuestion(0, 'Do you like new challenges & opportunities?');
g.addQuestion(1, 'Do you speak any European language?');
g.addQuestion(2, 'Are you interested in finance or IT?');
g.addQuestion(3, 'Which sector is more interesting for you?');
g.addQuestion(4, 'Do you have experience working with IT?');
g.addQuestion(5, 'Would you like to learn a new language?');
g.addAnswer(6, 'It\'s not a problem! Check our career page for the future possibilities.');
g.addAnswer(7, 'Congrats! We have an offer only for you!');
g.addAnswer(8, 'Great! Check out our career page for IT sector jobs.');
g.addAnswer(9, 'It\'s not a problem! We have an offer only for you!');

g.addEdge(0, 1, "Yes");
g.addEdge(1, 2, "Yes");
g.addEdge(2, 3, "Yes");
g.addEdge(4, 8, "Yes");
g.addEdge(5, 3, "Yes");
g.addEdge(0, 5, "No");
g.addEdge(5, 6, "No");
g.addEdge(1, 5, "No");
g.addEdge(4, 9, "No");
g.addEdge(3, 4, "IT");
g.addEdge(3, 7, "Finance");

g.setCurr(0);
export default g;