import React from 'react';
import { Text, View, StyleSheet, Image, Alert } from 'react-native';
import { StackNavigator } from 'react-navigation';
import Button from 'apsl-react-native-button';

export default class Prize extends React.Component {
    constructor() {
        super();
        this.state = { 
            purchase: {},
            loading: true
        };
    }

    componentDidMount() {
        console.log("Getting purchase");
        this.setState({
            purchaseId: this.props.navigation.state.purchaseId
        });
        console.log(this.state.purchaseId);
        this.getPurchase(this.props.navigation.state.purchaseId);
    }

    render() {
        const { params } = this.props.navigation.state;
        this.getPurchase(params.purchaseId);

        var button = this.state.purchase.isGivenAway ?
                        <Text>Already given away</Text>
                        :
                        <Button onPress={() => { this.giveAway(params.purchaseId) }}> Give away </Button>;
        return (
            
            <View style={styles.container}>
                <Button onPress={() => { this.goBack() }}> Back </Button>
                <Text>{params.purchaseId}</Text>
                <Text>Prize name: {this.state.purchase.name}</Text>
                <Text>Price: {this.state.purchase.price}</Text>
                <Text>User: {this.state.purchase.userEmail}</Text>
                <Image style={{height: 250}} source={{uri: this.state.purchase.picture }} />

                { this.state.loading ? <Text>Loading...</Text> :
                    button
                }
                
            </View>
        );
    }

    getPurchase(purchaseId) {
        fetch('https://cts-contest.azurewebsites.net/api/Purchase/GetPrizeByPurchaseGuid/' + purchaseId, {
            method: 'GET'
        })
        .then(response => response.json())
        .then(data => {
            this.setState({
                purchase: data,
                loading: false
            });
        }).catch(error => this.goBackOnError(error));
    }

    goBackOnError(error) {
        console.log(error);

        Alert.alert(
            'No purchase with given QR code',
            'You will be redirected back to QR code scanner',
            [
                {text: 'OK', onPress: () => this.goBack() },
            ],
            { cancelable: false }
        )
    }

    giveAway(purchaseId) {
        console.log(purchaseId);

        const formData = new FormData();
        formData.append('Id', purchaseId);
        fetch('https://cts-contest.azurewebsites.net/api/Purchase/GiveAway', {
            method: 'POST',
            body: formData
        })
        .then(response => response.json())
        .then(data => {
            console.log("Give away");
            console.log(data);

            if (data.isGivenAway) {
                Alert.alert(
                    'Successfully given away',
                    'You will be redirected back to QR code scanner',
                    [
                        {text: 'OK', onPress: () => console.log('OK Pressed')},
                    ],
                    { cancelable: false }
                )
            } else {
                Alert.alert(
                    'Prize was already given away',
                    'You will be redirected back to QR code scanner',
                    [
                        {text: 'OK', onPress: () => console.log('OK Pressed')},
                    ],
                    { cancelable: false }
                )
            }

            this.goBack();
        });

    }

    goBack() {
        this.props.navigation.state.params.resumeScanning();
        this.props.navigation.goBack();
    }
}


const styles = StyleSheet.create({
  container: {
    flex: 1,

    justifyContent: 'center',
    backgroundColor: '#fff',
  },
  bottomBar: {
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
    backgroundColor: 'rgba(0,0,0,0.5)',
    padding: 15,
    flexDirection: 'row',
  },
  url: {
    flex: 1,
  },
  urlText: {
    color: '#000',
    fontSize: 20,
  },
  cancelButton: {
    marginLeft: 10,
    alignItems: 'center',
    justifyContent: 'center',
  },
  cancelButtonText: {
    color: 'rgba(255,255,255,0.8)',
    fontSize: 18,
  },
});