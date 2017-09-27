// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

import * as React from 'react';
import { Responsive, Button, Header, Icon, Modal, Image, Loader, Container, Dimmer } from 'semantic-ui-react';
import { QRCode } from 'react-qr-svg';

interface PurchaseModalProps {
    open: boolean;
    onClose: any;
    prize: Prize;
    state: string;
    purchaseId: string;
}

interface PurchaseModalState {
}

export class PurchaseModal extends React.Component<PurchaseModalProps, PurchaseModalState> {
    modal: any;
    modalHeight: number;
    modalHeightSet = false;
    counter: number;
    constructor() {
        super();

    }
    componentDidUpdate(prevProps: PurchaseModalProps) {
        this.modalFix(prevProps);
    }
    modalFix(prevProps: PurchaseModalProps) {
        if (prevProps.state !== this.props.state) {
            this.counter = 0;
        }
        if (this.modal.ref !== undefined && this.modal.ref !== null) {
            this.modalHeight = 0;
            for (let i = 0; i < this.modal.ref.children.length; i++) {
                this.modalHeight += this.modal.ref.children[i].clientHeight;
            }
            if (this.counter <= 2) {
                this.setState({});
                const body = document.getElementsByTagName('body')[0];
                if(this.state !== 'loading')
                    body.classList.add('scrolling');
                this.counter++;
            }
        }
    }
    handleResize = () => {
        this.setState({});
    }
    close = () => {
        this.props.onClose();
    }
    public renderError() {
        return (
            <Modal.Content>
                <Container textAlign='center'>
                    <Header>An error has occurred!</Header>
                </Container>
            </Modal.Content>
        )
    }
    public renderLoading() {
        return (
            <Dimmer active={this.props.state === 'loading' ? true : false} /*onClickOutside={this.close}*/ page>
                <Loader active inline='centered' inverted={false} >Loading</ Loader>
            </Dimmer>
        )
    }
    public renderHeader() {
        return (
            <Modal.Header>
                {this.props.prize.name}
            </Modal.Header>
        )
    }
    public renderQRImage() {
        return (
            <Modal.Content>
                <QRCode
                    bgColor="#FFFFFF"
                    fgColor="#000000"
                    level="Q"
                    style={{ width: '100%' }}
                    value={this.props.purchaseId !== undefined ? this.props.purchaseId : ''}
                />
            </Modal.Content>
        )
    }
    public renderContent() {
        return (
            <Modal.Content>
                <Container textAlign='center'>
                    <Header>Your item has been purchased</Header>
                </Container>
            </Modal.Content>
        )
    }
    public renderActions() {
        return (
            <Modal.Actions>
                <Button content='Close' onClick={this.close} />
            </Modal.Actions>
        )
    }
    public render() {
        if(this.props.state === 'loading')
            return this.renderLoading();
        return (
            <Responsive onUpdate={this.handleResize}>
                <Modal open={this.props.open}
                    size='tiny'
                    closeOnEscape={true}
                    closeOnRootNodeClick={true}
                    style={{ height: window.innerHeight >= this.modalHeight ? this.modalHeight : 'auto' }}
                    onClose={this.close}
                    ref={(content: any) => this.modal = content}
                >
                    {this.renderHeader()}
                    
                    {this.props.state === 'loaded'
                        ? this.renderQRImage()
                        : ''
                    }
                    {this.props.state === 'loaded'
                        ? this.renderContent()
                        : ''
                    }
                    {this.props.state === 'error'
                        ? this.renderError()
                        : ''
                    }
                    {this.props.state !== 'loading'
                        ? this.renderActions()
                        : ''
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