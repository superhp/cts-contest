import * as React from 'react';
import { RouteComponentProps } from 'react-router';
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
        return <table className='table'>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Cost</th>
                    <th>Left</th>
                </tr>
            </thead>
            <tbody>
                {prizes.map(prize =>
                    <tr key={prize.id}>
                        <td>{prize.name}</td>
                        <td>{prize.cost}</td>
                        <td>{prize.leftCount}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }
}

interface Prize {
    id: number,
    cost: number,
    leftCount: number,
    name: string
}
