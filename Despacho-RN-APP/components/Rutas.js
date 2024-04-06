import React, { useState, useEffect } from 'react';
import { StyleSheet, View, Image, TouchableOpacity, ScrollView } from 'react-native';
import { TextInput, Text, Dialog, Button, DataTable, Modal } from 'react-native-paper';
import axios from 'axios';
import { BASE_URL_API, API_KEY} from '../Config';
import { Dropdown } from 'react-native-element-dropdown';
import AntDesign from '@expo/vector-icons/AntDesign';
import { Audio } from 'expo-av';
import AsyncStorage from '@react-native-async-storage/async-storage';

const Rutas = (props) => {

    const [messg, setMessg] = React.useState('');
    const showDialog = () => setVisible(true);
    const hideDialog = () => setVisible(false);
    const [visible, setVisible] = React.useState(false);
    const [open, setOpen] = useState(false);
    const [value, setValue] = useState();
    const [consecutivo, setConsecutivo] = React.useState('');
    const [scan, setScan] = React.useState();
    const [data, setData] = React.useState();
    const routes = [
        { label: 'CARTAGO', value: 'CARTAGO' },
        { label: 'DESAMPARADOS', value: 'DESAMPARADOS' },
        { label: 'GUADA/TIBAS/SP', value: 'GUADA' },
        { label: 'HEREDIA', value: 'HEREDIA' },
        { label: 'TRANSPORTE', value: 'TRANSPORTE' },
        { label: 'TRANSMEDICAL', value: 'TRANSMEDICAL' },
        { label: 'CAMBRONERO', value: 'CAMBRONERO' },
        { label: 'CARTAGO CERVAN', value: 'CERVANTES' },
        { label: 'RECOGEN', value: 'RECOGEN' },
    ];
    const [sound, setSound] = React.useState();
    const containerStyle = {backgroundColor: 'white', padding: 30, margin:10};
    const [conductor, setConductor] = React.useState();
    const [visibleDriver, setVisibleDriver] = React.useState(false);
    const showModalDriver = () => setVisibleDriver(true);
    const hideModalDriver = () => setVisibleDriver(false);

    const newInfoDespacho = async() =>{
        if(value == null || value == ''){
            setMessg('No se especificado una ruta en especifico');
            showDialog();
        }
        else{
            await axios.post(`${BASE_URL_API}Despacho/createNewDespacho`,
            {
                ruta: value
            },
            {
                headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('key')}`},
                params: {
                    ruta: value
                }
            })
            .then(function(response){
                if(response.status==200){
                    setConsecutivo(response.data);
                }
            })
            .catch(function(err){
                setMessg(err.response.request._response);
                showDialog();
            });
        }
    } 

    const insertNewScan = async (scanInfo)=>{
        await axios.post(`${BASE_URL_API}Despacho/insertNewScan`,
        {
            pedido: scanInfo,
            consecutivo: consecutivo
        },
        {
            headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('key')}`},
            params:{
                pedido: scanInfo,
                consecutivo: consecutivo
            }
        })
        .then(function(response){
            if(response.status == 200){
                setScan('');
                scanDataInfo();
            }
        })
        .catch(function(err){
            playSound();
            setScan('');
            setMessg(err.response.request._response);
            showDialog();
        });
    }

    const scanDataInfo = async() =>{
        if(consecutivo != null ||consecutivo != '' || consecutivo != undefined){
            await axios.get(`${BASE_URL_API}Despacho/getListScans`, {
                headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('key')}`},
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
    }

    const finishDespacho = async(conductor)=>{
        if(consecutivo=='' || consecutivo == null || consecutivo== undefined){
            await playSound();
            setMessg("No se puede finalizar un pedido sin antes haber generado un consecutivo de ruta");
            showDialog();
        }else{
            await axios.patch(`${BASE_URL_API}Despacho/finishDespacho`,
            {
                consecutivo: consecutivo,
                conductor: conductor
            },
            {
                headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('key')}`},
                params:{
                    consecutivo: consecutivo,
                    conductor: conductor
                }
            })
            .then(function(response){
                if(response.status == 200){
                    setConsecutivo('');
                    setValue();
                    hideModalDriver();
                    props.navigation.navigate("Inicio");
                }
            })
            .catch(function(err){
                playSound();
                setMessg(err.response.request._response);
                showDialog();
                setConductor('');
                hideModalDriver();
            });
        }
    }

    const deleteOrderScan = async(id) =>{
        await axios.delete(`${BASE_URL_API}Despacho/deleteScan?id=${id}`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('key')}`} })
        .then(function(response){
            if(response.status == 200){
                scanDataInfo();
            }
        })
        .catch(function(err){
            setMessg(err.message);
            showDialog();
        });
    }

    async function playSound() {
        const { sound } = await Audio.Sound.createAsync( require('../assets/wrong-answer.mp3')
        );
        setSound(sound);
        await sound.playAsync();
    }

    useEffect(() => {
        return sound
            ? () => {
                sound.unloadAsync();
            }
        : undefined;
    }, [sound]);

    useEffect(()=>{
        scanDataInfo();
    },[consecutivo]);

    useEffect(() => {
        const onFocus = async () => {
            scanDataInfo();
        };
        const focusPage = props.navigation.addListener('focus', onFocus);
        return focusPage;
    }, [props.navigation]); 

    return(
        <ScrollView>
            <View style={styles.header}>
                <View style={styles.row}>
                    <Image style={styles.image} source={require("../assets/Condefa.png")} />
                </View>
            </View>
            <View style={styles.content}>
                <View>
                    <View style={styles.row}>     
                        <Dropdown
                            style={styles.dropdown}
                            placeholderStyle={styles.placeholderStyle}
                            selectedTextStyle={styles.selectedTextStyle}
                            inputSearchStyle={styles.inputSearchStyle}
                            iconStyle={styles.iconStyle}
                            data={routes}
                            search
                            maxHeight={300}
                            labelField="label"
                            valueField="value"
                            placeholder="Seleccione una ruta"
                            searchPlaceholder="Buscar..."
                            value={value}
                            onChange={item => {
                                setValue(item.value);
                            }}
                            renderLeftIcon={() => (
                                <AntDesign style={styles.icon} color="black" name="Safety" size={20} />
                            )}
                        />
                        <TouchableOpacity style={styles.btnCancelar} onPress={() => newInfoDespacho()} >
                            <Text style={styles.btnText}>Nueva</Text> 
                        </TouchableOpacity>
                    </View>
                </View>
                <View>
                    <Text style={styles.rutatxt}>Consecutivo de ruta: {consecutivo} </Text>
                </View>
                <View>
                    <TextInput style={styles.textScanner} placeholder='Etiqueta' value={scan} onChangeText={(scan) => insertNewScan(scan) }></TextInput>
                </View>
                <View>
                    <DataTable style={styles.table}>
                        <DataTable.Header>
                            <DataTable.Title>Pedido</DataTable.Title>
                            <DataTable.Title>Bulto</DataTable.Title>
                            <DataTable.Title>Tipo</DataTable.Title>
                            <DataTable.Title>Registrado</DataTable.Title>
                            <DataTable.Title>Eliminar</DataTable.Title>
                        </DataTable.Header>
                        { (data !=null && data !='' && data != undefined) && data.map(item => ( 
                            <DataTable.Row>
                                <DataTable.Cell>{item.pedido}</DataTable.Cell>
                                <DataTable.Cell>{item.bulto}</DataTable.Cell>
                                <DataTable.Cell>{item.tipo}</DataTable.Cell>
                                <DataTable.Cell>{item.registrado}</DataTable.Cell>
                                <DataTable.Cell>
                                    <TouchableOpacity style={styles.btnEliminar} onPress={() => deleteOrderScan(item.id)} >
                                        <Text style={styles.btnText}>Eliminar</Text> 
                                    </TouchableOpacity>
                                </DataTable.Cell>
                            </DataTable.Row> )
                            ) 
                        }
                    </DataTable>
                </View>
                <View>
                    <TouchableOpacity style={styles.btnNuevo} onPress={() => showModalDriver()} >
                        <Text style={styles.btnText}>Finalizar</Text> 
                    </TouchableOpacity>
                </View>
                <Modal visible={visibleDriver} onDismiss={hideModalDriver} contentContainerStyle={containerStyle}>
                    <TextInput mode='outlined' 
                        secureTextEntry={true}  
                        value={conductor} 
                        label="Conductor" 
                        onChangeText={(conductor) => finishDespacho(conductor)}></TextInput>
                    <TouchableOpacity style={styles.btnDetalle}>
                        <Text style={styles.btnText}>Aplicar</Text>
                    </TouchableOpacity>
                </Modal>
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

export default Rutas;