import React, { useState, useEffect } from 'react';
import { StyleSheet, View, Image, TouchableOpacity, ScrollView } from 'react-native';
import { TextInput, Text, Dialog, Button, DataTable  } from 'react-native-paper';
import axios from 'axios';
import { BASE_URL_API, API_KEY} from '../Config';
import AsyncStorage from '@react-native-async-storage/async-storage';

const Detalle = (props)=>{
    
    const [data, setData] = React.useState();
    const [messg, setMessg] = React.useState('');
    const showDialog = () => setVisible(true);
    const hideDialog = () => setVisible(false);
    const [visible, setVisible] = React.useState(false);

    useEffect(() => {
        const onFocus = () => {
            getDataInfo();
        };
        const focusPage = props.navigation.addListener('focus', onFocus);
        return focusPage;
    }, [props.navigation]); 

    useEffect(()=>{
        getDataInfo();        
    }, []);

    const getDataInfo = async() =>{
        await axios.get(`${BASE_URL_API}Despacho/getListBultos`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('key')}`} } ,  {
            params: {
                consecutivo: (consecutivo==null ||consecutivo=='' || consecutivo== undefined)?0:consecutivo
            }
        })
        .then(function(response){
            if(response.status==200){
                setData(response.data.despachos);
            }
        })
        .catch(function(err){
            if(err.response.request._response != '0'){
                setMessg(err.response.request._response);
                showDialog();
            }
        })
    }

    return(
        <ScrollView>
            <View style={styles.header}>
                <View style={styles.row}>
                    <Image style={styles.image} source={require("../assets/Condefa.png")} />
                </View>
            </View>
            <View style={styles.content}>
                <View>
                    <DataTable style={styles.table}>
                        <DataTable.Header>
                            <DataTable.Title>Pedido</DataTable.Title>
                            <DataTable.Title>Bulto</DataTable.Title>
                            <DataTable.Title>Escaneado</DataTable.Title>
                        </DataTable.Header>
                        { (data !=null && data !='' && data != undefined) && data.map(item => ( 
                            <DataTable.Row>
                                <DataTable.Cell>{item.pedido}</DataTable.Cell>
                                <DataTable.Cell>{item.bulto}</DataTable.Cell>
                                <DataTable.Cell>{item.escaneado}</DataTable.Cell>
                            </DataTable.Row> )
                            )
                        }
                    </DataTable>
                </View>
            </View>
            <Dialog visible={visible} onDismiss={hideDialog}>
                <Dialog.Title>Mensaje</Dialog.Title>
                <Dialog.Content>
                    <Text variant="bodyMedium">{messg}</Text>
                </Dialog.Content>
                <Dialog.Actions>
                <Button onPress={hideDialog}>Ok</Button>
                </Dialog.Actions>
            </Dialog>
        </ScrollView>
    );
}

const styles = StyleSheet.create({
    header: {
        flex: 1,
        backgroundColor: 'white',
        alignItems: 'center',
        height: 130
    },
    colorBackgroud:{
        backgroundColor: 'white',
    },
    content:{
        height:'100%',
        width:'100%',
        flex: 1,
        backgroundColor: 'white'
    },
    row:{
        width:'100%',
        height: '15%',
        flexDirection: 'row',
        flexWrap: 'wrap',
    },
    image :{
        width:120,
        resizeMode: 'contain',
        marginLeft:20,
    },
    textScanner:{
        width: '95%',
        marginLeft:10,
        height: 50,
        marginTop: 50,
        marginBottom:20
    },
    btnNuevo: {
        width: "95%",
        height: 50,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 10,
        marginLeft: 10,
        backgroundColor: "#235271",
    },
    btnEliminar: {
        width: "100%",
        height: 30,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 10,
        marginLeft: 10,
        backgroundColor: "#235271",
    },
    btnCancelar: {
        width: "25%",
        height: 50,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 10,
        marginLeft: 10,
        backgroundColor: "#235271",
    },
    btnText: {
        color: "white",
    },
    dropdown: {
        margin: 16,
        height: 50,
        width: "60%",
        borderBottomColor: 'gray',
        borderBottomWidth: 0.5,
    },
    icon: {
        marginRight: 5,
    },
    placeholderStyle: {
        fontSize: 16,
    },
    selectedTextStyle: {
        fontSize: 16,
    },
    iconStyle: {
        width: 20,
        height: 20,
    },
    inputSearchStyle: {
        height: 40,
        fontSize: 16,
    },
    table:{
        marginBottom:20
    },
    rutatxt:{
        marginTop: 40,
        marginLeft: 15
    }
});

export default Detalle;