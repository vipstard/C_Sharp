$('.editor').trumbowyg({
    lang: 'ko',
    btnsDef: { //  버튼을 재정의 할 수 있다.
        image: { 
            dropdown: ['insertImage', 'upload'], //정의된 내용이라 마음대로 바꾸면 안된다. 
            ico:'insertImage'
        }
    },
    btns: [
        ['viewHTML'],
        ['undo', 'redo'], // Only supported in Blink browsers
        ['formatting'],
        ['strong', 'em', 'del'],
        ['superscript', 'subscript'],
        ['link'],
        'image',
        ['insertImage'],
        ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
      
    ]
});
