// cut down version for the LMS project, predominently used for the treeview.
declare module kendo {

    interface IDataSource {
        id: number;
        text: string;
        expanded?: boolean;
        spriteCssClass?: string;
        items?: IDataSource[];
    }
}