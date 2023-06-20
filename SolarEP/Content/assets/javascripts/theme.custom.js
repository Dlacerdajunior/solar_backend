/* Add here all your JS customizations */
var pathname = window.location.pathname
$('#menu > ul > li> a').parent('li').removeClass('nav-active')
$('#menu > ul > li> a[href="'+pathname+'"]').parent('li').addClass('nav-active')

namelink = $('#menu > ul > li.nav-active>a').attr('namelink');
// console.log(namelink)


// return false


$('body > section > div > section:nth-child(3) > header > h2').html(namelink)
$('body > section > div > section:nth-child(3) > header > div > ol > li:nth-child(2) > span').html(namelink)