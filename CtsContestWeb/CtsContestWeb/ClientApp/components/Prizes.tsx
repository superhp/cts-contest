import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Grid } from 'semantic-ui-react';
//https://react.semantic-ui.com/usage stylesheet missing
import { PrizeModal } from './PrizeModal';
import { PrizeCard } from './PrizeCard';
import 'isomorphic-fetch';



interface PrizesState {
    prizes: Prize[];
    loading: boolean;
    prizeModalLoading: boolean;
    prizeModalOpen: boolean;
    prizeModalData: Prize;
}

export class Prizes extends React.Component<RouteComponentProps<{}>, PrizesState> {
    constructor() {
        super();
        this.state = {
            prizes: [], loading: true,
            prizeModalLoading: true,
            prizeModalOpen: false,
            prizeModalData: {
                id: 0,
                name: "",
                price: 0,
                quantity: 0,
                picture: ""
            }
        };

        fetch('api/Prize')
            .then(response => response.json() as Promise<Prize[]>)
            .then(data => {
                this.setState({ prizes: data, loading: false });
            });
    }
    closePrizeModal = () => {
        this.setState({
            prizeModalLoading: true,
            prizeModalOpen: false,
            prizeModalData: {
                id: 0,
                name: "",
                price: 0,
                quantity: 0,
                picture: ""
            }
        });
    }

    buy = (id: number) => {
        this.setState({prizeModalOpen: true});
        // fetch('api/Prize/' + id)
        //     .then(response => response.json() as Promise<Prize>)
        //     .then(data => {
        //         this.setState({ prizeModalData: data, prizeModalLoading: false });
        //     });
        const prize = this.state.prizes.filter((pr: Prize) => {return pr.id === id})[0];
        this.setState({ prizeModalData: prize, prizeModalLoading: false });
    }
    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderForecastsTable(this.state.prizes);

        return <div>
            <h1>Prizes</h1>
            <p>Buy some stuff!</p>
            {contents}
            <PrizeModal open={this.state.prizeModalOpen} onClose={this.closePrizeModal} prize={this.state.prizeModalData} loading={this.state.prizeModalLoading} />
        </div>;
    }

    private renderForecastsTable(prizes: Prize[]) {
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
                            <PrizeCard id={prize.id} name={prize.name} picture={prize.picture} quantity={prize.quantity} price={prize.price} onBuy={this.buy}/>
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
