import React, { useState, useEffect } from 'react';
import { StyleSheet, View, Image, TouchableOpacity, ScrollView } from 'react-native';
import { TextInput, Text, Dialog, Button, DataTable } from 'react-native-paper';
import axios from 'axios';
import { BASE_URL_API, API_KEY} from '../Config';
import AsyncStorage from '@react-native-async-storage/async-storage';

const Home = (props) => {

    const [messg, setMessg] = React.useState('');
    const showDialog = () => setVisible(true);
    const hideDialog = () => setVisible(false);
    const [visible, setVisible] = React.useState(false);
    const [data, setData] = React.useState();
    
    const getDespachosInfo = async () =>{
        await axios.get(`${BASE_URL_API}Despacho/getRouteList`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('key')}`} })
        .then(function(response){
            if(response.status == 200){
                setData(response.data);
            }
        })
        .catch(function(err){
            setMessg(err.response.request._response);
            showDialog();
        })
    }

    useEffect(()=>{
        getDespachosInfo();        
    }, []);

    useEffect(() => {
        const onFocus = () => {
            getDespachosInfo();
        };
        const focusPage = props.navigation.addListener('focus', onFocus);
        return focusPage;
    }, [props.navigation]); 

    return(
        <ScrollView automaticallyAdjustKeyboardInsets={true}>
            <View style={styles.header}>
                <View style={styles.row}>
                    <Image style={styles.image} source={require("../assets/Condefa.png")} />
                </View>
            </View>
            <View style={styles.content}>
                <View>
                    <DataTable>
                        <DataTable.Header>
                            <DataTable.Title>Consecutivo</DataTable.Title>
                            <DataTable.Title style={{flex: 2}}>Ruta</DataTable.Title>
                            <DataTable.Title>Cerrada</DataTable.Title>
                        </DataTable.Header>
                        { (data !=null && data !='' && data != undefined) && data.map(item => ( 
                            <DataTable.Row>
                                <DataTable.Cell>{item.consecutivo}</DataTable.Cell>
                                <DataTable.Cell style={{flex: 2}}>{item.ruta}</DataTable.Cell>
                                <DataTable.Cell>{(item.finalizado)? 'Si': 'No'}</DataTable.Cell>
                            </DataTable.Row> )
                            ) 
                        }
                    </DataTable>
                </View>
                <View style={styles.row}>
                    <TouchableOpacity style={styles.btnNuevo} onPress={() => props.navigation.navigate("Rutas")} >
                        <Text style={styles.btnText}>Nuevo despacho</Text> 
                    </TouchableOpacity>                    
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
        width: '90%',
        marginLeft:20,
        height: 40
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
    btnCancelar: {
        width: "25%",
        height: 50,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 10,
        marginLeft: 10,
        backgroundColor: "#Ff0000",
    },
    btnText: {
        color: "white",
    },
});

export default Home;