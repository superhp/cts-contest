// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

import * as React from 'react';
import { Responsive, Button, Header, Icon, Modal, Image, Grid, Loader, Segment } from 'semantic-ui-react';

interface PrizeModalProps {
    open: boolean;
    onClose: any;
    prize: Prize;
    loading: boolean;
}

interface PrizeModalState {
    modalHeight: any;
    modalImageWidth: any;
}

const height = 300;

export class PrizeModal extends React.Component<PrizeModalProps, PrizeModalState> {
    constructor() {
        super();

        let modalHeight = height + 100;
        if (window.innerWidth < 768)
            modalHeight = window.innerHeight - 50;
        this.state = {
            modalHeight: modalHeight,
            modalImageWidth: 400
        }
    }
    handleResize = () => {
        if (window.innerWidth < 768)
            this.setState({ modalHeight: window.innerHeight - 50, modalImageWidth: '100%' });
        else
            this.setState({ modalHeight: height + 100, modalImageWidth: 400 });
    }
    public renderLoading() {
        return <Loader active inline='centered' inverted={false} />
    }
    public renderHeader() {
        return (
            <Modal.Header>
                {this.props.prize.name}
                <div style={{ float: 'right' }}>
                    Balance: 255 <Icon name='money' />
                </div>
            </Modal.Header>
        )
    }
    public renderPrize() {
        return (
            <Modal.Content image scrolling>
                <div style={{ height: height, maxWidth: this.state.modalImageWidth }}>
                    <Image src={/*"http://cts-contest-cms.azurewebsites.net/" + */this.props.prize.picture} height={height} centered />
                </div>

                <Modal.Description>
                    <Grid centered columns={1}>
                        <Header>Do you really want to buy?</Header>
                    </Grid>
                    <Grid centered columns={2}>
                        <Grid.Column>
                            <Button onClick={this.props.onClose}>No</Button>
                        </Grid.Column>
                        <Grid.Column>
                            <Button positive label={this.props.prize.price} icon='shop' content="Buy" labelPosition='left' />
                        </Grid.Column>
                    </Grid>
                </Modal.Description>
            </Modal.Content>
        )
    }
    public render() {
        return (
            <Responsive onUpdate={this.handleResize}>
                <Modal open={this.props.open}
                    dimmer='blurring'
                    size='large'
                    closeOnEscape={true}
                    closeOnRootNodeClick={true}
                    style={{ height: this.state.modalHeight, maxHeight: window.innerHeight - 50 }}
                    onClose={this.props.onClose}
                    closeIcon
                >
                    {/* {this.renderLoading()} */}
                     {this.props.loading
                        ? this.renderLoading()
                        : this.renderHeader()
                    }
                    {this.props.loading
                        ?''
                        : this.renderPrize()
                    } 
                </Modal>
            </Responsive>
        )
    }
}

export interface Prize {
    id: number;
    name: string;
    price: number;
    quantity: number;
    picture: string;
}