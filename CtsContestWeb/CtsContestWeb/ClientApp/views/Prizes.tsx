import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Grid, Container, Header, Icon, Loader, Divider } from 'semantic-ui-react';
//https://react.semantic-ui.com/usage stylesheet missing
import { PrizeDescription } from '../components/PrizeDescription';
import { PrizeModal } from '../components/PrizeModal';
import { PurchaseModal } from '../components/PurchaseModal';
import { PrizeCard } from '../components/PrizeCard';
import 'isomorphic-fetch';

// import * as GA from 'react-ga';
// GA.initialize('UA-109707377-1');

export class Prizes extends React.Component<any, any> {
    _mounted: boolean;
    constructor() {
        super();
        this.state = {
            prizes: [], loading: true,
            prizeDescription: {},
            prizeDescriptionOpen: false
        };

    }

    componentWillMount() {
        //GA.pageview(window.location.pathname + window.location.search);
    }

    componentDidMount() {
        fetch('api/Prize/GetWinnables')
            .then(response => response.json() as Promise<Prize[]>)
            .then(data => {
                this.setState({ prizes: data, loading: false });
            });
        this._mounted = true;
    }

    componentWillUnmount() {
        this._mounted = false;
    }
    /*
     * Description modal
    */

    openDescriptionModal = (prize: any) => {
        this.setState({ prizeDescriptionOpen: true, prizeDescription: prize });
    }

    closeDescriptionModal = () => {
        this.setState({ prizeDescriptionOpen: false });
    }
    public render() {
        let contents = this.state.loading
            ? <Loader active inline='centered'>Loading</Loader>
            : this.renderPrizeList(this.state.prizes.filter((prize: any) => prize.category !== 'Prize for points'));

        return (
            <div className='cg-prize-page'>
                <div className='cg-page-header'>
                    <Container fluid>
                        <Header as='h1' textAlign='center' inverted>
                            <Icon name='gift' />
                            <Header.Content>
                                Prize
                            </Header.Content>
                        </Header>
                    </Container>
                </div>
                <Container>
                    {contents}
                    <PrizeDescription
                        open={this.state.prizeDescriptionOpen}
                        onClose={this.closeDescriptionModal}
                        prize={this.state.prizeDescription} />
                </Container>

            </div>
        );
    }
    private renderPrizeList(prizes: Prize[]) {
        const dayPrizes = [];
        dayPrizes.push(prizes.filter((prize: any) => prize.category.toLowerCase() === 'wednesday prize')[0]);
        dayPrizes.push(prizes.filter((prize: any) => prize.category.toLowerCase() === 'thursday prize')[0]);
        dayPrizes.push(prizes.filter((prize: any) => prize.category.toLowerCase() === 'friday prize')[0]);
        
        const conferencePrize = prizes.filter((prize: any) => prize.category.toLowerCase() === 'week prize');//[];
        //conferencePrize.push(prizes.find((prize: any) => prize.category.toLowerCase() === 'week prize'));
        return (
            <div>
                <div className='cg-row'>
                    {conferencePrize.map((prize, index) =>
                        <div className='cg-col main-prize-prizes' key={index} style={{ paddingBottom: 20 }}>
                            <PrizeCard
                                prize={prize}
                                onOpenDescription={this.openDescriptionModal}
                            />
                        </div>
                    )}
                </div>
                {/*<div className='cg-row last-not-grow'>
                    {dayPrizes.map((prize, index) =>
                        <div className='cg-col' key={index} style={{ paddingBottom: 20 }}>
                            <PrizeCard
                                prize={prize}
                                onOpenDescription={this.openDescriptionModal}
                            />
                        </div>
                    )}
                </div>*/}
            </div>
        )
    }
}

interface Prize {
    id: number;
    price: number;
    quantity: number;
    name: string;
    picture: string;
    category: string;
}

interface Purchase {
    id: string;
}