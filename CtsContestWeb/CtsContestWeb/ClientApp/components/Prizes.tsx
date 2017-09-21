import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Grid } from 'semantic-ui-react';
//https://react.semantic-ui.com/usage stylesheet missing
//import { Prize } from './PrizeCard';
import { PrizeCard } from './PrizeCard';
import 'isomorphic-fetch';



interface PrizesState {
    prizes: Prize[];
    loading: boolean;
}

export class Prizes extends React.Component<RouteComponentProps<{}>, PrizesState> {
    constructor() {
        super();
        this.state = { prizes: [], loading: true };

        fetch('api/Prize')
            .then(response => response.json() as Promise<Prize[]>)
            .then(data => {
                this.setState({ prizes: data, loading: false });
            });
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Prizes.renderForecastsTable(this.state.prizes);

        return <div>
            <h1>Prizes</h1>
            <p>Buy some stuff!</p>
            {contents}
        </div>;
    }

    private static renderForecastsTable(prizes: Prize[]) {
        var groups = new Array<Array<Prize>>();
        prizes.forEach((val, i) => {
            var idx = Math.floor(i / 4);
            if (groups.length <= idx) {
                groups.push(new Array<Prize>());
            }
            groups[idx].push(val)
        });
        return <Grid columns={4}>
            {groups.map((group, rowIndex) =>
                <Grid.Row key={rowIndex}>
                    {group.map((prize, colIndex) =>
                        <Grid.Column key={colIndex}>
                            <PrizeCard name={prize.name} picture={prize.picture} quantity={prize.quantity} price={prize.price} />
                        </Grid.Column>
                        )}
                </Grid.Row>
                )}
        </Grid>;
    }
}

interface Prize {
    id: number;
    price: number;
    quantity: number;
    name: string;
    picture: string;
}
