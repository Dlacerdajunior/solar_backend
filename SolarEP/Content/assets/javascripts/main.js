$(document).ready( function() {
     alert('main')     
	/*Ativando plugin dataTables*/
	$('#table-scroll').dataTable( {
		"scrollXInner": "100%",
		"oLanguage": {
                    "sProcessing":   "Processando...",
                    "sLengthMenu":   "Mostrar _MENU_ registros",
                    "sZeroRecords":  "Não foram encontrados resultados",
                    "sInfo":         "Mostrando de _START_ até _END_ de _TOTAL_ registros",
                    "sInfoEmpty":    "Mostrando de 0 até 0 de 0 registros",
                    "sInfoFiltered": "",
                    "sInfoPostFix":  "",
                    "sUrl":          ""
                    }
	} );



} );