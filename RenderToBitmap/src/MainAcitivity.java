import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;

import net.sf.andpdf.nio.ByteBuffer;

import com.sun.pdfview.PDFFile;
import com.sun.pdfview.PDFPage;
import com.unity3d.player.UnityPlayerActivity;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.RectF;
import android.os.Bundle;




public class MainAcitivity extends UnityPlayerActivity 
{
	public static Context mContext;
	
	@Override
    public void onCreate(Bundle bundle)
    {
        super.onCreate(bundle);
        mContext = this;
    }
	
	/**
	 * Use this to render a pdf file given as InputStream to a Bitmap.
	 * 
	 * @param context
	 *            current context.
	 * @param inStream
	 *            the inputStream of the pdf file.
	 * @return a bitmap.
	 * @see https://github.com/jblough/Android-Pdf-Viewer-Library/
	 */
	@Nullable
	public static Bitmap renderToBitmap(byte[] data, String Folder) {
	    Bitmap bi = null;
	    FileOutputStream out = null;
	    try 
	    {
	    	
	        ByteBuffer buf = ByteBuffer.wrap(data);
	        PDFFile pdfFile = new PDFFile(buf);
	        File cacheDir = mContext.getCacheDir();
	        for(int i =0; i < pdfFile.getNumPages(); i++)
	        {
	        	PDFPage mPdfPage = pdfFile.getPage(i);
	        	float width = mPdfPage.getWidth();
	        	float height = mPdfPage.getHeight();
	        	RectF clip = null;
	        	bi = mPdfPage.getImage((int) (width), (int) (height), clip, true, true); 
	        	File temp = File.createTempFile("slide" + i, "png", cacheDir);
	            out = new FileOutputStream(temp);	
	        	bi.compress(Bitmap.CompressFormat.PNG, 100, out);
	        }
	    } catch (IOException e) {
	        e.printStackTrace();
	    }
	    return bi;
	}

}
