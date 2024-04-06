import React, { useState, useEffect, useRef  } from 'react';
import { StyleSheet, View, Image, TouchableOpacity, ScrollView } from 'react-native';
import { TextInput, Text, Modal, DataTable, RadioButton, Dialog, Button } from 'react-native-paper';
import axios from 'axios';
import { BASE_URL_API, API_KEY} from '../Config';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Audio } from 'expo-av';

const PedidoComponent = (props) => {
    
    const artScan = useRef(null);
    const [codigo, codigoState] = React.useState('');
    const [bod, bodState] = React.useState('');
    const [loc, locState] = React.useState('');
    const [traza, trazaState] = React.useState('');
    const [tipo, tipoState] = React.useState('');
    const [cantPend, cantPendState] = React.useState('');
    const [cantEsc, cantEscState] = React.useState('');
    const [prod, prodState] = React.useState('');
    const [articulo, setArticulo] = React.useState('');
    const [linea, setLinea] = React.useState('');
    const [visible, setVisible] = React.useState(false);
    const [renglon, setRenglon] = React.useState(0);
    const [almacenMos, setAlmacenMos] = React.useState(false);
    const [artTemp, setArtTemp] = React.useState('');

    const [visibleAlert, setVisibleAlert] = React.useState(false);
    const showDialog = () => setVisibleAlert(true);
    const hideDialog = () => setVisibleAlert(false);
    const [messg, setMessg] = React.useState('');

    const [visibleAlertG, setVisibleAlertG] = React.useState(false);
    const showDialogG = () => setVisibleAlertG(true);
    const hideDialogG = () => setVisibleAlertG(false);
    const [messgG, setMessgG] = React.useState('');

    const [visibleAlertY, setVisibleAlertY] = React.useState(false);
    const showDialogY = () => setVisibleAlertY(true);
    const hideDialogY = () => setVisibleAlertY(false);
    const [messgY, setMessgY] = React.useState('');

    const [visibleAlertR, setVisibleAlertR] = React.useState(false);
    const showDialogR = () => setVisibleAlertR(true);
    const hideDialogR = () => setVisibleAlertR(false);
    const [messgR, setMessgR] = React.useState('');

    const [data, setData] = React.useState([]);
    const [pedido, setPedido] =React.useState();
    const [justificacion, setJustificacion] = React.useState('');
    const [trazable, setTrazable] = React.useState('');
    const [sound, setSound] = React.useState();
    const [user, setUser] = React.useState();

    const [value, setValue] = React.useState();
    const [SuperUser, setSuperUser] = React.useState();

    const showModal = () => setVisible(true);
    const hideModal = () => setVisible(false);

    const [visibleBocom, setVisibleBocom] = React.useState(false);
    const showModalBocom = () => setVisibleBocom(true);
    const hideModalBocom = () => setVisibleBocom(false);

    const [visibleUser, setVisibleUser] = React.useState(false);
    const [visibleUserOCC, setVisibleUserOCC] = React.useState(false);

    const showModalUser = () => {
        setVisibleUser(true);
        setSuperUser('');
    };
    const showModalUserOCC = () => {
        setVisibleUserOCC(true);
        setVisibleUserOCC('');
    };

    const hideModalUser = () => setVisibleUser(false);
    const hideModalUserOCC = () => setVisibleUserOCC(false);

    const containerStyle = {backgroundColor: 'white', padding: 30, margin:10};

    const saveDataInfo = async () =>{
        setPedido(await AsyncStorage.getItem('pedido'));
        setUser(await AsyncStorage.getItem('username'));
    }

    const getArticleInfo = async (articulo) =>{
        await axios.get(`${BASE_URL_API}Orders/getStorageData?articulo=${articulo}`, { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} })
        .then(function(response){
            if(response.status == 200){
                setData(response.data);
            }
        }).catch(function(err){
            setMessg(err.response.request._response);
            showDialog();
        });
    } 

    async function playSound() {
        const { sound } = await Audio.Sound.createAsync( require('../assets/wrong-answer-2.mp3')
        );
        setSound(sound);
        await sound.playAsync();
    }

    async function playSoundSuccess() {
        const { sound } = await Audio.Sound.createAsync( require('../assets/success.mp3')
        );
        setSound(sound);
        await sound.playAsync();
    }

    React.useEffect(() => {
        return sound
            ? () => {
                sound.unloadAsync();
            }
        : undefined;
    }, [sound]);

    const getOrderInfo = async () =>{
        if(visibleAlert){
            hideDialog();
            hideDialogY();
            hideDialogR();
            hideDialogG();
        }
        else{
            await saveDataInfo();
            setValue();
            var renglon = await AsyncStorage.getItem('renglon');
            if(renglon == undefined || renglon == null){
                renglon = 0;
            }
            await axios.get(`${BASE_URL_API}Orders/getOrderDetailsComplete?pedido=${await AsyncStorage.getItem('pedido')}&usuario=${await AsyncStorage.getItem('username')}&renglonID=${renglon}&next=${false}`, 
                { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} })
            .then(function(response){
                if(response.status == 200){
                    if(response.data != null && response.data != undefined ){
                        if(response.data.bod != bod && (bod != null && bod != undefined && bod != '')){
                            setMessg('El almacen de esta linea es distinta a los anteriores, favor validar bien la bodega a la que pertenece');
                            showDialog();
                        }
                        if(response.data.cantidadAlmacenes == 1 && response.data.bod == '112' && ! almacenMos){
                            setAlmacenMos(true);
                            setMessg('Este pedido solo tiene articulos de la bodega 112, favor tener cuidado al alistar');
                            showDialog();
                        }
                        setArticulo(response.data.articulo);
                        setLinea(response.data.linea);
                        bodState(response.data.bod);
                        locState(response.data.loc);
                        trazaState(response.data.traza);
                        tipoState(response.data.tipo);
                        cantPendState(response.data.cantidadPedido.toString());
                        cantEscState(response.data.cantidadEscaneado.toString());
                        prodState(response.data.producto);
                        setTrazable(response.data.trazable);
                        getArticleInfo(response.data.articulo);
                        const setLine = async () =>{
                            await AsyncStorage.setItem('renglon', response.data.renglon2.toString());
                            await setRenglon(response.data.renglon2);
                        }
                        setLine();
                    }
                    else{
                        setArticulo('XXXXXX');
                        setLinea('XXXXXX');
                        bodState('XXXXXX');
                        locState('XXXXXX');
                        trazaState('XXXXXX');
                        tipoState('XXXXXX');
                        cantPendState('XXXXXX');
                        cantEscState('XXXXXX');
                        prodState('XXXXXX');
                        setRenglon('XXXXXX');
                    }
                }
                else{
                    setArticulo('XXXXXX');
                    setLinea('XXXXXX');
                    bodState('XXXXXX');
                    locState('XXXXXX');
                    trazaState('XXXXXX');
                    tipoState('XXXXXX');
                    cantPendState('XXXXXX');
                    cantEscState('XXXXXX');
                    prodState('XXXXXX');
                    setRenglon('XXXXXX');
                    setPedido(null);
    
                }
            }).catch(function(err){
                setArticulo('XXXXXX');
                setLinea('XXXXXX');
                bodState('XXXXXX');
                locState('XXXXXX');
                trazaState('XXXXXX');
                tipoState('XXXXXX');
                cantPendState('XXXXXX');
                cantEscState('XXXXXX');
                prodState('XXXXXX');
                setRenglon('XXXXXX');
                setPedido(null);
            });
        }

    }

    const NextLine = async () =>{
        if(visibleAlert || visibleAlertY || visibleAlertR || visibleAlertG){
            hideDialog();
            hideDialogY();
            hideDialogR();
            hideDialogG();
        }
        else{
            var renglon = await AsyncStorage.getItem('renglon');
            if(renglon == undefined || renglon == null){
                renglon = 0;
            }
            await axios.get(`${BASE_URL_API}Orders/getOrderDetailsComplete?pedido=${await AsyncStorage.getItem('pedido')}&usuario=${await AsyncStorage.getItem('username')}&renglonID=${renglon}&next=${true}`, 
                { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} })
            .then(function(response){
                if(response.status == 200){
                    if(response.data != null && response.data != undefined ){
    
                        if(response.data.bod != bod && (bod != null && bod != undefined && bod != '')){
                            setMessg('El almacen de esta linea es distinta a los anteriores, favor validar bien la bodega a la que pertenece');
                            showDialog();
                        }
                        if(response.data.cantidadAlmacenes == 1 && response.data.bod == '112' && ! almacenMos){
                            setAlmacenMos(true);
                            setMessg('Este pedido solo tiene articulos de la bodega 112, favor tener cuidado al alistar');
                            showDialog();
                        }
    
                        setArticulo(response.data.articulo);
                        setLinea(response.data.linea);
                        bodState(response.data.bod);
                        locState(response.data.loc);
                        trazaState(response.data.traza);
                        tipoState(response.data.tipo);
                        cantPendState(response.data.cantidadPedido.toString());
                        cantEscState(response.data.cantidadEscaneado.toString());
                        prodState(response.data.producto);
                        setTrazable(response.data.trazable);
                        getArticleInfo(response.data.articulo);
                        const setLine = async () =>{
                            await AsyncStorage.setItem('renglon', response.data.renglon2.toString());
                            await setRenglon(response.data.renglon2);
                        }
                        setLine();
                    }
                    else{
                        setArticulo('');
                        setLinea('');
                        bodState('');
                        locState('');
                        trazaState('');
                        tipoState('');
                        cantPendState('');
                        cantEscState('');
                        prodState('');
                        setRenglon('');
                    }
                }
                else{
                    setArticulo('');
                    setLinea('');
                    bodState('');
                    locState('');
                    trazaState('');
                    tipoState('');
                    cantPendState('');
                    cantEscState('');
                    prodState('');
                    setRenglon('');
                    setPedido(null);
                }
            }).catch(function(err){
                setArticulo('');
                setLinea('');
                bodState('');
                locState('');
                trazaState('');
                tipoState('');
                cantPendState('');
                cantEscState('');
                prodState('');
                setPedido(null);
            });
        }

    }

    const validateExpiredDate = async (dataScan) =>{
        setArtTemp(dataScan);
        if( (trazable == 'Serie' || trazable.toLowerCase() == 'serie') && dataScan != 'PSWBOCON'){
            await axios.get(`${BASE_URL_API}Orders/expiredDateMonths?articulo=${dataScan}&id=${await AsyncStorage.getItem('pedido')}`, 
                { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} })
            .then(function(response){
                validateScan(dataScan);
            })
            .catch(function(err){
                let validate = err.response.data.meses;
                if(validate <= 4){
                    if(validate == 1){
                        setMessgR(`Este producto está proximo a vencer, faltan ${err.response.data.meses} mes para el vencimiento del producto`);
                        showDialogR();
                    }
                    else{
                        setMessgR(`Este producto está proximo a vencer, faltan ${err.response.data.meses} meses para el vencimiento del producto`);
                        showDialogR();
                    }
                }
                else if(validate <= 8 && validate > 4){
                    setMessgY(`Este producto está proximo a vencer, faltan ${err.response.data.meses} meses para el vencimiento del producto`);
                    showDialogY();
                }
                else{
                    setMessgG(`Este producto está proximo a vencer, faltan ${err.response.data.meses} meses para el vencimiento del producto`);
                    showDialogG();
                }
            })
        }
        else{
            validateScan(dataScan);
        }
    }

    const validateScan = async (dataScan) =>{
        dataScan = artTemp;
        if(dataScan.length < 2){
            codigoState('');
        }
        else if(dataScan == "PSWBOCON" && (pedido == undefined || pedido == null)){
            setMessg('No existe información asociada para realizar un bocon');
            showDialog();
        }
        else if(dataScan == "PSWBOCON" && (pedido != undefined && pedido != null) ){
            codigoState('');
            showModalBocom();
        }
        else if(dataScan==null || dataScan==undefined || dataScan==''){
            await playSound();
            setMessg('No se ha ingresado ningun articulo');
            showDialog();
        }
        else{
            await axios.patch(`${BASE_URL_API}Orders/updateScanOrder`, 
            {
                id: pedido,
                renglon: linea,
                art: dataScan,
                tipo: trazable,
                usuario: await AsyncStorage.getItem('username')
            },
            {
                headers:{
                    Authorization: `Bearer ${await AsyncStorage.getItem('token')}`
                },
                params:{
                    id: pedido,
                    renglon: linea,
                    art: dataScan,
                    tipo: trazable,
                    usuario: await AsyncStorage.getItem('username')
                }
            })
            .then(function(response){
                if(response.status==200){
                    hideDialogR();
                    hideDialogG();
                    hideDialogY();
                    hideDialog();
                    const updateData = async ()=>{
                        await getOrderInfo();
                        await playSoundSuccess();
                    }      
                    updateData();
                    codigoState('');
                }
            }).catch(function(err){
                hideDialogY();
                hideDialogR();
                hideDialogG();
                const showDialogError = async ()=>{
                    await playSound();
                    setMessg(err.response.request._response);
                    showDialog();
                }
                showDialogError();
            });
        }
    }

    const changeOrderStatus = async () =>{
        var pedido = await AsyncStorage.getItem('pedido');
        await axios.patch(
            `${BASE_URL_API}Orders/updateSituation`,
                {
                    pedido: pedido,
                    situation: 'En Empaque',
                    superUser: SuperUser,
                    usuario: await AsyncStorage.getItem('username')
                },
                {
                    headers:{
                        Authorization: `Bearer ${await AsyncStorage.getItem('token')}`
                    },
                    params:{
                        pedido: pedido,
                        situation: 'En Empaque',
                        superUser: SuperUser,
                        usuario: await AsyncStorage.getItem('username')
                    }
                }
            ).then(function (response){
                if(response.status == 200){
                    hideModalUser();
                    const removeKeyAsync = async() =>{
                        await AsyncStorage.removeItem('pedido');
                        setPedido('');
                        setSuperUser('');
                        props.navigation.navigate("Inicio");
                    }
                    removeKeyAsync();
                }
            }).catch(function(err){
                const showDialogError = async ()=>{
                    await playSound();
                    setMessg(err.response.request._response);
                    showDialog();
                }
                showDialogError();
            });
    }

    const validateSuperUserNeeded = async () =>{
        await axios.get(`${BASE_URL_API}Orders/getValidateBocon?pedido=${await AsyncStorage.getItem('pedido')}`, 
            { headers: {"Authorization" : `Bearer ${await AsyncStorage.getItem('token')}`} })
        .then(function(response){
            if(response.status == 200){
                if(response.data.estado){
                    showModalUser();
                }
                else{
                    changeOrderStatus('N/A');
                }
            }
        });
    }

    const cancelOrden = async() =>{
        await axios.post(
            `${BASE_URL_API}Orders/saveJustifyBocom`,
            {
                id: pedido,
                renglon: linea,
                tipo: value,
                usuario: SuperUser,
                justificacion: justificacion
            },
            {
                headers:{
                    Authorization: `Bearer ${await AsyncStorage.getItem('token')}`
                },
                params:{
                    id: pedido,
                    renglon: linea,
                    tipo: value,
                    usuario: SuperUser,
                    justificacion: justificacion
                }
            }
        ).then(function(response){
            if(response.status == 200){
                prodState('');
                setPedido('');
                codigoState('');
                setRenglon('');
                setJustificacion('');
                hideModalUser();
                hideModalBocom();
                props.navigation.navigate("Inicio");
            }
        }).catch(function(err){
            setMessg(err.response.request._response);
            showDialog();
        });
    }

    const saveJustifyBocom = async () =>{
        if(justificacion == undefined || justificacion == null || justificacion == ""){
            await playSound();
            setMessg('El campo de justificación es obligatorio');
            showDialog();
        }
        if(value == "OCC"){
            showModalUserOCC();
        }
        else{
            await axios.post(
                `${BASE_URL_API}Orders/saveJustifyBocom`,
                {
                    id: pedido,
                    renglon: linea,
                    tipo: value, 
                    usuario: 'N/A',
                    justificacion: justificacion
                },
                {
                    headers:{
                        Authorization: `Bearer ${await AsyncStorage.getItem('token')}`
                    },
                    params:{
                        id: pedido,
                        renglon: linea,
                        tipo: value, 
                        usuario: 'N/A',
                        justificacion: justificacion
                    }
                }
            ).then(function(response){
                if(response.status == 200){
                    setJustificacion('');
                    codigoState('');
                    hideModalBocom();
                    hideModalUser();
                    setJustificacion('');
                    const movBocon = async ()=>{
                        await getOrderInfo();
                    }
                    movBocon();
                }
            }).catch(function(err){
                setMessg(err.response.request._response);
                showDialog();
            });
        }
    }

    useEffect(() => {
        const onFocus = async () => {
            await getOrderInfo();
            setAlmacenMos(false);
            artScan.current.focus();
        };
        const focusPage = props.navigation.addListener('focus', onFocus);
        return focusPage;
    }, [props.navigation]); 

    return(
        <ScrollView automaticallyAdjustKeyboardInsets={true}>
            <View style={styles.header}>
                <View style={styles.content}>
                    <View style={styles.row}>
                        <TextInput style={styles.inputTextLine} ref={artScan} mode="flat" blurOnSubmit={true} showSoftInputOnFocus={false} value={codigo} onChangeText={(codigo) => validateExpiredDate(codigo)}></TextInput>
                    </View>
                    <View style={styles.rowLabels}>
                        <Text variant='titleLarge' style={styles.labelText}>Articulo:</Text>
                        <Text variant='titleLarge' style={styles.labelArticulo} onPress={showModal} >{articulo}</Text>
                        <Text variant='titleLarge' style={styles.labelText}>Lin</Text>
                        <Text variant='titleLarge' style={styles.labelText}>{linea}</Text>
                    </View>
                    <View style={styles.rowInputs}>
                        <TextInput mode="underline" theme={{colors: {primary: 'red'}}} multiline={true} numberOfLines={5} style={styles.inputTextArea} label="Producto" editable={false} value={prod} />
                    </View>
                    <View style={styles.rowInputs1}>
                        <TextInput mode="underline" textColor='white' style={(bod=='112')? styles.inputTextSegmentbod1:styles.inputTextSegmentbod2} label="BOD" editable={false} value={bod} onChangeText={()=>{bodState(bod)}} />
                        <TextInput mode="underline" style={styles.inputTextSegment} label="LOC" editable={false} value={loc} onChangeText={()=> {locState(loc)}} />
                    </View>
                    <View style={styles.rowInputs}>
                        <TextInput mode="underline" style={styles.inputTextSegment} label="TRAZA" editable={false} value={traza} onChangeText={()=> {trazaState(traza)}} />
                        <TextInput mode="underline" style={styles.inputTextSegment} label="TIPO" editable={false} value={tipo} onChangeText={()=> {tipoState(tipo)}} />
                    </View>
                    <View style={styles.rowInputs}>
                        <TextInput mode="underline" textColor='white' style={styles.inputTextSegment2} label="Cant_Ped" editable={false} value={cantPend} onChangeText={()=> cantPendState(cantPend)} />
                        <TextInput mode="underline" textColor='white' style={styles.inputTextSegment2} label="Cant_Esc" editable={false} value={cantEsc} onChangeText={()=> cantEscState(cantEsc)}/> 
                    </View>
                    <View style={styles.row}>
                        <TouchableOpacity style={styles.btnDetalle} onPress={ () => NextLine() }>
                            <Text style={styles.btnText}>Siguiente línea</Text> 
                        </TouchableOpacity>
                    </View>
                    <View style={styles.row}>
                        <TouchableOpacity style={styles.btnDetalleFin} onPress={ () => validateSuperUserNeeded() }>
                            <Text style={styles.btnText}>Finalizar</Text> 
                        </TouchableOpacity>
                    </View>
                </View>
                <Modal visible={visible} onDismiss={hideModal} contentContainerStyle={containerStyle}>
                    <Text>Articulo:</Text>
                    <Text>{articulo}</Text>
                    <DataTable>
                        <DataTable.Header>
                            <DataTable.Title>BOD</DataTable.Title>
                            <DataTable.Title>DISP</DataTable.Title>
                            <DataTable.Title>REST</DataTable.Title>
                        </DataTable.Header>
                        {data.map(item =>(
                            <DataTable.Row key={item.articulo}>
                                <DataTable.Cell>{item.bodega}</DataTable.Cell>
                                <DataTable.Cell>{item.disponible}</DataTable.Cell>
                                <DataTable.Cell>{item.reservado}</DataTable.Cell>
                            </DataTable.Row>
                        ))}
                    </DataTable>
                    <TouchableOpacity style={styles.btnDetalle} onPress={hideModal}>
                        <Text style={styles.btnText}>OK</Text>  
                    </TouchableOpacity>
                </Modal>
                <Modal visible={visibleBocom} onDismiss={hideModalBocom} contentContainerStyle={containerStyle}>
                    <RadioButton.Group onValueChange={value => setValue(value)} value={value}>
                        <RadioButton.Item label="Faltante de productos" value="FDP" />
                        <RadioButton.Item label="Productos con vencimiento" value="PCV" />
                        <RadioButton.Item label="Cancelar orden" value="OCC" />
                    </RadioButton.Group>
                    <TextInput mode='outlined' value={justificacion} onChangeText={(justificacion) => setJustificacion(justificacion)}></TextInput>
                    <TouchableOpacity style={styles.btnDetalle} onPress={() => saveJustifyBocom()}>
                        <Text style={styles.btnText}>Aplicar</Text> 
                    </TouchableOpacity>
                </Modal>
                <Modal visible={visibleUser} onDismiss={hideModalUser} contentContainerStyle={containerStyle}>
                    <TextInput mode='outlined' secureTextEntry={true} value={SuperUser} label="Usuario supervisor" onChangeText={(SuperUser) => setSuperUser(SuperUser)}></TextInput>
                    <TouchableOpacity style={styles.btnDetalle} onPress={ () => changeOrderStatus()}>
                        <Text style={styles.btnText}>Aplicar</Text>
                    </TouchableOpacity>
                </Modal>
                <Modal visible={visibleUserOCC} onDismiss={hideModalUserOCC} contentContainerStyle={containerStyle}>
                    <TextInput mode='outlined' secureTextEntry={true} value={SuperUser} label="Usuario supervisor" onChangeText={(SuperUser) => setSuperUser(SuperUser)}></TextInput>
                    <TouchableOpacity style={styles.btnDetalle} onPress={ () => cancelOrden()}>
                        <Text style={styles.btnText}>Aplicar</Text>
                    </TouchableOpacity>
                </Modal>
            </View>
            <Dialog visible={visibleAlert} onDismiss={hideDialog}>
                <Dialog.Title>Mensaje</Dialog.Title>
                <Dialog.Content>
                    <Text variant="bodyMedium">{messg}</Text>
                </Dialog.Content>
                <Dialog.Actions>
                    <Button onPress={hideDialog}>Ok</Button>
                </Dialog.Actions>
            </Dialog>

            <Dialog style={styles.colorDialogG} visible={visibleAlertG} onDismiss={hideDialogG}>
                <Dialog.Title>Mensaje</Dialog.Title>
                <Dialog.Content>
                    <Text variant="bodyMedium">{messgG}</Text>
                </Dialog.Content>
                <Dialog.Actions>
                    <Button onPress={hideDialogG}>Cancelar envÍo</Button>
                    <Button onPress={ (codigo) => validateScan(codigo)}>Enviar producto</Button>
                </Dialog.Actions>
            </Dialog>

            <Dialog style={styles.colorDialogY} visible={visibleAlertY} onDismiss={hideDialogY}>
                <Dialog.Title>Mensaje</Dialog.Title>
                <Dialog.Content>
                    <Text variant="bodyMedium">{messgY}</Text>
                </Dialog.Content>
                <Dialog.Actions>
                <Button onPress={hideDialogY}>Cancelar envÍo</Button>
                    <Button onPress={(codigo) => validateScan(codigo)}>Enviar producto</Button>
                </Dialog.Actions>
            </Dialog>

            <Dialog style={styles.colorDialogR} visible={visibleAlertR} onDismiss={hideDialogR}>
                <Dialog.Title>Mensaje</Dialog.Title>
                <Dialog.Content>
                    <Text variant="bodyMedium">{messgR}</Text>
                </Dialog.Content>
                <Dialog.Actions>
                    <Button onPress={hideDialogR}>Cancelar envÍo</Button>
                    <Button onPress={(codigo) => validateScan(codigo)}>Enviar producto</Button>
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
    },
    colorDialogG:{
        backgroundColor:'#C1F2B0',
        color: 'white'
    },
    colorDialogY:{
        backgroundColor:'#FFF7D4',
        color: 'white'
    },
    colorDialogR:{
        backgroundColor:'#EF4040',
        color: 'white'
    },
    content:{
        width:'100%',
        flex: 1,
        alignItems: 'center',
    },
    row:{
        width:'100%',
        height: '15%',
        flexDirection: 'row',
        flexWrap: 'wrap',
    },
    rowLabels:{
        width:'100%',
        height: '7%',
        flexDirection: 'row',
        flexWrap: 'wrap',
    },
    rowInputs1:{
        width:'100%',
        height: '7%',
        flexDirection: 'row',
        flexWrap: 'wrap',
    }, 
    rowInputs:{
        width:'100%',
        height: '7%',
        flexDirection: 'row',
        flexWrap: 'wrap',
        marginTop: 20,
    }, 
    image :{
        width:120,
        resizeMode: 'contain',
        marginLeft:20,
    },
    homeText: {
        color: 'black',
        marginTop: 20,
        fontSize: 20,
    },
    btnMenu: {
        width: "30%",
        height: 50,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 10,
        marginLeft: 10,
        backgroundColor: "#235271",
    },
    btnDetalle: {
        width: "95%",
        height: 50,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 70,
        marginLeft: 10,
        backgroundColor: "#235271",
    },
    btnDetalleFin: {
        width: "95%",
        height: 50,
        alignItems: "center",
        justifyContent: "center",
        marginTop: 20,
        marginLeft: 10,
        backgroundColor: "green",
    },
    btnText: {
        color: "white",
    },
    inputTextLine:{
        height: 50,
        marginTop: 40,
        marginLeft: 10,
        width: '95%'
    },
    inputTextArea:{
        backgroundColor: 'white',
        height: 80,
        marginTop: 0,
        marginLeft: 10,
        width: '95%',
    },
    inputTextSegment:{
        backgroundColor: 'white',
        height: 60,
        marginTop: 40,
        marginLeft: 10,
        width: '46%',
        fontSize: 23
    },
    inputTextSegment2:{
        backgroundColor: 'orange',
        height: 60,
        color: 'white',
        marginTop: 40,
        marginLeft: 10,
        width: '46%',
        fontSize: 23
    },
    inputTextSegmentbod1:{
        backgroundColor: 'red',
        color: 'white',
        height: 60,
        marginTop: 40,
        marginLeft: 10,
        width: '46%',
        fontSize: 23
    },
    inputTextSegmentbod2:{
        backgroundColor: 'green',
        color: 'white',
        height: 60,
        marginTop: 40,
        marginLeft: 10,
        width: '46%',
        fontSize: 23
    },
    labelText:{
        marginTop: 0,
        marginLeft: 15,
    },
    labelArticulo: {
        marginTop: 0,
        paddingLeft: 5,
        paddingRight: 5,
        marginLeft: 15,
        color: 'white',
        fontWeight:'bold',
        backgroundColor: 'red'
    }
});

export default PedidoComponent;