    public enum SessionState
    {
        disabled,       //何も準備されてない
        prepairing,     //ボード準備中
        actionSelect,        //アクション選択待ち
        patchInput,     //パッチ入力待ち
        patchPlace,      //パッチ置き待ち
        patchMeasure,
        patchReform,
        reformSubmit
    }