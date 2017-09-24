// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

import * as React from 'react';
import { Responsive, Button, Header, Icon, Modal } from 'semantic-ui-react';

interface PrizeModalProps {
    open: boolean;
    onClose: any;
    prize: Prize;
}

interface PrizeModalState {
    modalHeight: any;
}

export class PrizeModal extends React.Component<PrizeModalProps, PrizeModalState> {
    constructor() {
        super();

        let modalHeight = 512;
        if (window.innerWidth < 768)
            modalHeight = window.innerHeight - 50;
        this.state = {
            modalHeight: modalHeight
        }
    }
    handleResize = () => {
        if (window.innerWidth < 768)
            this.setState({ modalHeight: window.innerHeight - 50 });
        else
            this.setState({ modalHeight: 512 });
    }

    public render() {
        return (
            <Responsive onUpdate={this.handleResize}>
                <Modal open={this.props.open}
                    size='large'
                    closeOnEscape={true}
                    closeOnRootNodeClick={true}
                    style={{ height: 'auto', maxHeight: this.state.modalHeight }}
                    onClose={this.props.onClose}
                    closeIcon
                >
                    <Modal.Header>{this.props.prize.name}</Modal.Header>
                    <Modal.Content scrolling>
                        <Modal.Description>
                            <Header>Default Profile Image</Header>
                            <p>We've found the following gravatar image associated with your e-mail address.
                            We've found the following gravatar image associated with your e-mail address.
                            We've found the following gravatar image associated with your e-mail address.
                            We've found the following gravatar image associated with your e-mail address.
                            We've found the following gravatar image associated with your e-mail address.
                            We've found the following gravatar image associated with your e-mail address.
                            We've found the following gravatar image associated with your e-mail address.
                            We've found the following gravatar image associated with your e-mail address.
                            We've found the following gravatar image associated with your e-mail address.
                        </p>
                            <p>Is it okay to use this photo?</p>
                        </Modal.Description>
                    </Modal.Content>
                    <Modal.Actions>
                        <Button color='green'>
                            <Icon name='checkmark' /> Buy
                        </Button>
                    </Modal.Actions>
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