import static com.google.common.base.Preconditions.checkArgument;
import static com.google.common.base.Preconditions.checkNotNull;
import static com.google.common.base.Preconditions.checkState;
import static org.apache.commons.lang3.StringUtils.isNotBlank;

import java.awt.Graphics2D;
import java.awt.Rectangle;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

import javax.imageio.ImageIO;

import org.springframework.batch.item.ItemProcessor;

import lombok.extern.java.Log;

@Log
public class PictureProcessor implements ItemProcessor<Picture, Picture> {

	private int x = 313;
	private int y = 145;
	private int width = 576 - x;
	private int height = 445 - y;

	private int w2 = 300;
	private int h2 = 300;

	@Override
	public Picture process(Picture item) throws Exception {
		checkArgument(isNotBlank(item.getSrc()));
		checkArgument(isNotBlank(item.getDest()));
		log.info(item.getSrc());
		log.info(item.getDest());
		BufferedImage origin = ImageIO.read(new File(item.getSrc()));
		BufferedImage croped = crop(origin, new Rectangle(x, y, width, height));
		BufferedImage resized = resize(croped, new Rectangle(0, 0, w2, h2));
		write(resized, item);
		return item;
	}

	private Picture write(BufferedImage resized, Picture item) throws IOException {
		checkNotNull(resized);
		File file = new File(item.getDest());
		if (!file.exists()) {
			file.getParentFile().mkdirs();
			file.createNewFile();
		}
		checkState(file.exists());
		ImageIO.write(resized, "png", file);
		return item;
	}

	private BufferedImage crop(BufferedImage img, Rectangle rect) {
		checkNotNull(img);
		checkNotNull(rect);
		BufferedImage res = img.getSubimage(rect.x, rect.y, rect.width, rect.height);
		return res;
	}

	// https://www.mkyong.com/java/how-to-resize-an-image-in-java/
	private BufferedImage resize(BufferedImage img, Rectangle rect) {
		checkNotNull(img);
		BufferedImage res = new BufferedImage(rect.width, rect.height, img.getType());
		Graphics2D g = res.createGraphics();
		g.drawImage(img, 0, 0, rect.width, rect.height, null);
		g.dispose();
		return res;
	}
}
