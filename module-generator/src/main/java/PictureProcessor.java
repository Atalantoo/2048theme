import static com.google.common.base.Preconditions.checkArgument;
import static org.apache.commons.lang3.StringUtils.isNotBlank;

import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.net.URI;

import javax.imageio.ImageIO;

import org.springframework.batch.item.ItemProcessor;
import org.springframework.core.io.Resource;

import com.google.common.base.Throwables;

import lombok.AllArgsConstructor;
import lombok.NonNull;

public class PictureProcessor implements ItemProcessor<Picture, Picture> {

	private int x = 313;
	private int y = 145;
	private int w = 576 - x;
	private int h = 445 - y;

	private int w2 = 300;
	private int h2 = 300;

	@Override
	public Picture process(Picture item) throws Exception {
		checkArgument(isNotBlank(item.getSrc()));
		checkArgument(isNotBlank(item.getDest()));
		BufferedImage origin = ImageIO.read(new File(item.getDest()));
		BufferedImage croped = crop(origin, x, y, w, h);
		BufferedImage scaled = scale(croped, w2, h2);
		ImageIO.write(scaled, "png", new File(item.getDest()));
		return item;
	}

	private BufferedImage crop(BufferedImage origin, int x2, int y2, int w3, int h3) {
		return null;
	}

	// https://www.mkyong.com/java/how-to-resize-an-image-in-java/
	private BufferedImage scale(BufferedImage originalImage, int w, int h) {
		BufferedImage resizedImage = new BufferedImage(w, h, originalImage.getType());
		Graphics2D g = resizedImage.createGraphics();
		g.drawImage(originalImage, 0, 0, w, h, null);
		g.dispose();
		return resizedImage;
	}
}
